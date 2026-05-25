export interface CriarAvaliacaoRequest {
  comentario: string;
  estrelas: number;
  usuarioId: number;
  estabelecimentoId: number;
}

export interface AtualizarAvaliacaoRequest {
  comentario: string;
  estrelas: number;
}

export interface AvaliacaoResponse {
  id: number;
  comentario: string;
  estrelas: number;
  usuarioId: number;
  ativo: boolean;
}
