import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AtualizarEstabelecimentoRequest,
  EstabelecimentoResponse,
  FiltroEstabelecimentoRequest,
} from '../models/estabelecimento.model';

@Injectable({ providedIn: 'root' })
export class EstabelecimentoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/estabelecimento`;

  criar(formData: FormData): Observable<EstabelecimentoResponse> {
    return this.http.post<EstabelecimentoResponse>(this.baseUrl, formData);
  }

  filtrar(filtro: FiltroEstabelecimentoRequest = {}): Observable<EstabelecimentoResponse[]> {
    let params = new HttpParams();

    if (filtro.nome) params = params.set('nome', filtro.nome);
    if (filtro.distanciaMaxima != null)
      params = params.set('distanciaMaxima', filtro.distanciaMaxima);
    if (filtro['geocordenadasRequest.latitude'] != null)
      params = params.set('geocordenadasRequest.latitude', filtro['geocordenadasRequest.latitude']!);
    if (filtro['geocordenadasRequest.longitude'] != null)
      params = params.set(
        'geocordenadasRequest.longitude',
        filtro['geocordenadasRequest.longitude']!
      );
    if (filtro['enderecoRequest.logradouro'])
      params = params.set('enderecoRequest.logradouro', filtro['enderecoRequest.logradouro']!);
    if (filtro['enderecoRequest.cidade'])
      params = params.set('enderecoRequest.cidade', filtro['enderecoRequest.cidade']!);
    if (filtro['enderecoRequest.uf'])
      params = params.set('enderecoRequest.uf', filtro['enderecoRequest.uf']!);
    if (filtro['enderecoRequest.cep'])
      params = params.set('enderecoRequest.cep', filtro['enderecoRequest.cep']!);
    if (filtro.recursosAcessabilidadeIds?.length) {
      filtro.recursosAcessabilidadeIds.forEach((id) => {
        params = params.append('recursosAcessabilidadeIds', id);
      });
    }

    return this.http.get<EstabelecimentoResponse[]>(this.baseUrl, { params });
  }

  getById(id: number): Observable<EstabelecimentoResponse> {
    return this.http.get<EstabelecimentoResponse>(`${this.baseUrl}/${id}`);
  }

  atualizar(
    id: number,
    request: AtualizarEstabelecimentoRequest
  ): Observable<EstabelecimentoResponse> {
    return this.http.patch<EstabelecimentoResponse>(`${this.baseUrl}/${id}`, request);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  uploadImagem(id: number, imagem: File): Observable<void> {
    const formData = new FormData();
    formData.append('imagem', imagem);
    return this.http.post<void>(`${this.baseUrl}/${id}/imagem`, formData);
  }
}
