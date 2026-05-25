import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AtualizarAvaliacaoRequest,
  AvaliacaoResponse,
  CriarAvaliacaoRequest,
} from '../models/avaliacao.model';

@Injectable({ providedIn: 'root' })
export class AvaliacaoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/avaliacao`;

  criar(request: CriarAvaliacaoRequest): Observable<AvaliacaoResponse> {
    return this.http.post<AvaliacaoResponse>(this.baseUrl, request);
  }

  getById(id: number): Observable<AvaliacaoResponse> {
    return this.http.get<AvaliacaoResponse>(`${this.baseUrl}/${id}`);
  }

  atualizar(id: number, request: AtualizarAvaliacaoRequest): Observable<AvaliacaoResponse> {
    return this.http.patch<AvaliacaoResponse>(`${this.baseUrl}/${id}`, request);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
