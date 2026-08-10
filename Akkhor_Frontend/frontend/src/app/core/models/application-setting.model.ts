
export interface ApplicationSetting {
  id?: string;

  key: string;

  value?: string | null;

  category: string;

  dataType: string;

  description?: string | null;

  isActive: boolean;

  createdAt?: string;

  updatedAt?: string | null;

  updatedBy?: string | null;
}

export interface CreateApplicationSetting {
  key: string;

  value?: string | null;

  category: string;

  dataType: string;

  description?: string | null;

  isActive: boolean;
}

export interface UpdateApplicationSetting {
  key: string;

  value?: string | null;

  category: string;

  dataType: string;

  description?: string | null;

  isActive: boolean;
}

