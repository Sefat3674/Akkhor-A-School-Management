
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  ApplicationSetting,
  UpdateApplicationSetting
} from '../../../../core/models/application-setting.model';

import {
  ApplicationSettingService
} from '../../../../core/services/application-setting.service';

@Component({
  selector: 'app-application-settings',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './application-settings.component.html',
  styleUrls: ['./application-settings.component.scss']
})
export class ApplicationSettingsComponent implements OnInit {

  // =====================================================
  // STATE
  // =====================================================

  settings: ApplicationSetting[] = [];

  loading = false;

  saving = false;

  errorMessage = '';

  successMessage = '';


  // =====================================================
  // CATEGORY ORDER
  // =====================================================

  readonly categoryOrder: string[] = [
    'General',
    'Assignment',
    'Notification',
    'Security',
    'System'
  ];


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private readonly applicationSettingService:
      ApplicationSettingService
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadSettings();
  }


  // =====================================================
  // LOAD SETTINGS
  // =====================================================

  loadSettings(): void {

    this.loading = true;

    this.errorMessage = '';
    this.successMessage = '';

    this.applicationSettingService
      .getAll()
      .subscribe({

        next: (response) => {

          this.settings = response ?? [];

          this.sortSettings();

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Error loading application settings:',
            error
          );

          this.errorMessage =
            error?.error?.message ??
            'Failed to load application settings.';

          this.loading = false;
        }

      });
  }


  // =====================================================
  // SORT SETTINGS
  // =====================================================

  private sortSettings(): void {

    this.settings.sort((a, b) => {

      const categoryA =
        this.categoryOrder.findIndex(
          category =>
            category.toLowerCase() ===
            a.category.toLowerCase()
        );

      const categoryB =
        this.categoryOrder.findIndex(
          category =>
            category.toLowerCase() ===
            b.category.toLowerCase()
        );

      const orderA =
        categoryA === -1
          ? 999
          : categoryA;

      const orderB =
        categoryB === -1
          ? 999
          : categoryB;

      if (orderA !== orderB) {
        return orderA - orderB;
      }

      return a.key.localeCompare(b.key);
    });
  }


  // =====================================================
  // GET SETTINGS BY CATEGORY
  // =====================================================

  getSettingsByCategory(
    category: string
  ): ApplicationSetting[] {

    return this.settings.filter(
      setting =>
        setting.category.toLowerCase() ===
        category.toLowerCase()
    );
  }


  // =====================================================
  // ADDITIONAL CATEGORIES
  // =====================================================

  getAdditionalCategories(): string[] {

    const categories = this.settings
      .map(setting => setting.category)
      .filter(
        (category): category is string =>
          !!category
      );

    const uniqueCategories =
      [...new Set(categories)];

    return uniqueCategories.filter(
      category =>
        !this.categoryOrder.some(
          orderedCategory =>
            orderedCategory.toLowerCase() ===
            category.toLowerCase()
        )
    );
  }


  // =====================================================
  // CATEGORY ICON
  // =====================================================

  getCategoryIcon(category: string): string {

    switch (category.toLowerCase()) {

      case 'general':
        return 'fas fa-school';

      case 'assignment':
        return 'fas fa-book-open';

      case 'notification':
        return 'fas fa-bell';

      case 'security':
        return 'fas fa-shield-alt';

      case 'system':
        return 'fas fa-cog';

      default:
        return 'fas fa-sliders-h';
    }
  }


  // =====================================================
  // CATEGORY DESCRIPTION
  // =====================================================

  getCategoryDescription(category: string): string {

    switch (category.toLowerCase()) {

      case 'general':
        return 'Configure your school information.';

      case 'assignment':
        return 'Control assignment and submission behavior.';

      case 'notification':
        return 'Configure system notification behavior.';

      case 'security':
        return 'Configure authentication and security settings.';

      case 'system':
        return 'Configure system-wide application behavior.';

      default:
        return 'Configure application settings.';
    }
  }


  // =====================================================
  // DISPLAY LABEL
  // =====================================================

  getSettingLabel(key: string): string {

    if (!key) {
      return '';
    }

    return key
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .replace(/_/g, ' ')
      .replace(/\b\w/g, char =>
        char.toUpperCase()
      );
  }


  // =====================================================
  // VALUE TYPE
  // =====================================================

  isBoolean(
    setting: ApplicationSetting
  ): boolean {

    return setting.dataType?.toLowerCase() === 'boolean';
  }


  isInteger(
    setting: ApplicationSetting
  ): boolean {

    return [
      'integer',
      'int',
      'number'
    ].includes(
      setting.dataType?.toLowerCase()
    );
  }


  isString(
    setting: ApplicationSetting
  ): boolean {

    return !this.isBoolean(setting)
      && !this.isInteger(setting);
  }


  // =====================================================
  // BOOLEAN VALUE
  // =====================================================

  getBooleanValue(
    setting: ApplicationSetting
  ): boolean {

    if (typeof setting.value === 'boolean') {
      return setting.value;
    }

    return String(setting.value)
      .toLowerCase() === 'true';
  }


  setBooleanValue(
    setting: ApplicationSetting,
    value: boolean
  ): void {

    setting.value =
      value ? 'true' : 'false';
  }


  // =====================================================
  // NUMBER VALUE
  // =====================================================

  getNumberValue(
    setting: ApplicationSetting
  ): number {

    const value =
      Number(setting.value);

    return Number.isNaN(value)
      ? 0
      : value;
  }


  setNumberValue(
    setting: ApplicationSetting,
    value: number
  ): void {

    setting.value =
      String(value);
  }


  // =====================================================
  // SAVE ALL
  // =====================================================

  saveAll(): void {

    if (
      this.saving ||
      this.settings.length === 0
    ) {
      return;
    }

    this.saving = true;

    this.errorMessage = '';
    this.successMessage = '';

    const updateRequests =
      this.settings.map(setting => {

        const dto: UpdateApplicationSetting = {

          key: setting.key,

          value: setting.value,

          category: setting.category,

          dataType: setting.dataType,

          description: setting.description,

          isActive: setting.isActive
        };

        return this.applicationSettingService
          .update(
            setting.id!,
            dto
          );
      });

    let completed = 0;

    let hasError = false;

    updateRequests.forEach(request => {

      request.subscribe({

        next: () => {

          completed++;

          if (
            completed ===
              updateRequests.length &&
            !hasError
          ) {

            this.saving = false;

            this.successMessage =
              'Application settings saved successfully.';

            this.clearSuccessMessage();
          }
        },

        error: (error) => {

          console.error(
            'Error updating application setting:',
            error
          );

          hasError = true;

          this.saving = false;

          this.errorMessage =
            error?.error?.message ??
            'Failed to save application settings.';
        }

      });

    });
  }


  // =====================================================
  // CLEAR SUCCESS MESSAGE
  // =====================================================

  private clearSuccessMessage(): void {

    setTimeout(() => {

      this.successMessage = '';

    }, 4000);
  }


  // =====================================================
  // RESET / RELOAD
  // =====================================================

  resetChanges(): void {

    if (this.saving) {
      return;
    }

    this.loadSettings();
  }


  // =====================================================
  // TRACK BY
  // =====================================================

  trackBySetting(
    index: number,
    setting: ApplicationSetting
  ): string | number {

    return setting.id ?? index;
  }
}

