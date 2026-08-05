import { Pipe, PipeTransform } from '@angular/core';
import {
  CaseType, CaseStatus, CasePriority,
  CASE_TYPE_LABELS, CASE_STATUS_LABELS, CASE_STATUS_CLASS, CASE_PRIORITY_LABELS
} from '../models/models';

/** Renders the numeric CaseType coming from the API as its display label, e.g. 2 -> "Family". */
@Pipe({ name: 'caseTypeLabel', standalone: true })
export class CaseTypeLabelPipe implements PipeTransform {
  transform(value: CaseType | null | undefined): string {
    return value == null ? '-' : CASE_TYPE_LABELS[value] ?? '-';
  }
}

/** Renders the numeric CaseStatus coming from the API as its display label, e.g. 3 -> "In Hearing". */
@Pipe({ name: 'caseStatusLabel', standalone: true })
export class CaseStatusLabelPipe implements PipeTransform {
  transform(value: CaseStatus | null | undefined): string {
    return value == null ? '-' : CASE_STATUS_LABELS[value] ?? '-';
  }
}

/** Maps the numeric CaseStatus to the CSS class suffix used by .status-* pill styles. */
@Pipe({ name: 'caseStatusClass', standalone: true })
export class CaseStatusClassPipe implements PipeTransform {
  transform(value: CaseStatus | null | undefined): string {
    return value == null ? '' : CASE_STATUS_CLASS[value] ?? '';
  }
}

/** Renders the numeric CasePriority coming from the API as its display label, e.g. 3 -> "Urgent". */
@Pipe({ name: 'casePriorityLabel', standalone: true })
export class CasePriorityLabelPipe implements PipeTransform {
  transform(value: CasePriority | null | undefined): string {
    return value == null ? '-' : CASE_PRIORITY_LABELS[value] ?? '-';
  }
}
