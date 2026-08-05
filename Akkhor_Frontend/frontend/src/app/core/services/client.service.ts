import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Client, CreateClient } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ClientService {
  private base = `${environment.apiUrl}/clients`;

  constructor(private http: HttpClient) {}

  getAll(search?: string): Observable<Client[]> {
    const params = search ? { params: { search } } : {};
    return this.http.get<Client[]>(this.base, params);
  }

  getById(id: number): Observable<Client> {
    return this.http.get<Client>(`${this.base}/${id}`);
  }

  create(dto: CreateClient): Observable<Client> {
    return this.http.post<Client>(this.base, dto);
  }

  update(id: number, dto: CreateClient): Observable<Client> {
    return this.http.put<Client>(`${this.base}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
