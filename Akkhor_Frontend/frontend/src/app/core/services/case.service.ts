import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CaseStatus, CreateCase, Hearing, LawCase } from '../models/models';

@Injectable({ providedIn: 'root' })
export class CaseService {
  private base = `${environment.apiUrl}/cases`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<LawCase[]> {
    return this.http.get<LawCase[]>(this.base);
  }

  getById(id: number): Observable<LawCase> {
    return this.http.get<LawCase>(`${this.base}/${id}`);
  }

  create(dto: CreateCase): Observable<LawCase> {
    return this.http.post<LawCase>(this.base, dto);
  }

  updateStatus(id: number, status: CaseStatus, judgmentResult?: string, compensationAmount?: number): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}/status`, { status, judgmentResult, compensationAmount });
  }

  getHearings(caseId: number): Observable<Hearing[]> {
    return this.http.get<Hearing[]>(`${this.base}/${caseId}/hearings`);
  }

  addHearing(caseId: number, hearingDate: string, court?: string, notes?: string): Observable<Hearing> {
    return this.http.post<Hearing>(`${this.base}/${caseId}/hearings`, { hearingDate, court, notes });
  }
}
