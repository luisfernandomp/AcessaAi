import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AtualizarUsuarioRequest, CadastrarUsuarioRequest, UsuarioResponse } from '../models/usuario.model';

@Injectable({ providedIn: 'root' })
export class UsuarioService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/usuario`;

  cadastrar(request: CadastrarUsuarioRequest): Observable<UsuarioResponse> {
    return this.http.post<UsuarioResponse>(`${this.baseUrl}/cadastrar`, request);
  }

  getById(id: number): Observable<UsuarioResponse> {
    return this.http.get<UsuarioResponse>(`${this.baseUrl}/${id}`);
  }

  atualizar(id: number, request: AtualizarUsuarioRequest): Observable<UsuarioResponse> {
    return this.http.put<UsuarioResponse>(`${this.baseUrl}/${id}`, request);
  }

  uploadFotoPerfil(id: number, arquivo: File): Observable<string> {
    const fd = new FormData();
    fd.append('foto', arquivo, arquivo.name);
    return this.http.post(`${this.baseUrl}/${id}/foto-perfil`, fd, { responseType: 'text' });
  }
}
