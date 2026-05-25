import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RecursoAcessibilidade } from '../models/recurso-acessibilidade.model';

@Injectable({ providedIn: 'root' })
export class RecursoAcessibilidadeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/recursoacessibilidade`;

  listarAtivas(): Observable<RecursoAcessibilidade[]> {
    return this.http.get<RecursoAcessibilidade[]>(`${this.baseUrl}/listar-ativas`);
  }
}
