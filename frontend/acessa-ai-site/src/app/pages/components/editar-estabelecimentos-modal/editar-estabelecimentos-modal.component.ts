import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EstabelecimentoService } from '../../../core/services/estabelecimento.service';
import { EstabelecimentoResponse } from '../../../core/models/estabelecimento.model';

@Component({
  selector: 'app-editar-estabelecimentos-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './editar-estabelecimentos-modal.component.html',
  styleUrls: ['./editar-estabelecimentos-modal.component.css'],
})
export class EditarEstabelecimentosModalComponent implements OnInit {
  @Output() fechar = new EventEmitter<void>();
  @Output() editar = new EventEmitter<EstabelecimentoResponse>();
  @Output() deletar = new EventEmitter<number>();

  private estabelecimentoService = inject(EstabelecimentoService);
  private toastr = inject(ToastrService);

  estabelecimentos: EstabelecimentoResponse[] = [];
  carregando = true;
  erro: string | null = null;

  ngOnInit(): void {
    this.carregarEstabelecimentos();
  }

  carregarEstabelecimentos(): void {
    this.carregando = true;
    this.erro = null;

    // Para buscar estabelecimentos do usuário, usamos o filtro sem parâmetros
    // Você pode precisar implementar um endpoint específico para isso
    this.estabelecimentoService.filtrar({}).subscribe({
      next: (dados) => {
        this.estabelecimentos = dados;
        this.carregando = false;
      },
      error: (err) => {
        console.error('Erro ao carregar estabelecimentos:', err);
        this.erro = 'Não foi possível carregar os estabelecimentos. Tente novamente.';
        this.carregando = false;
      },
    });
  }

  onEditar(estabelecimento: EstabelecimentoResponse): void {
    this.editar.emit(estabelecimento);
  }

  onDeletar(id: number, nome: string): void {
    if (confirm(`Tem certeza que deseja deletar "${nome}"?`)) {
      this.estabelecimentoService.deletar(id).subscribe({
        next: () => {
          this.estabelecimentos = this.estabelecimentos.filter((e) => e.id !== id);
          this.deletar.emit(id);
        },
        error: () => {
          this.toastr.error('Não foi possível deletar o estabelecimento. Tente novamente.');
        },
      });
    }
  }

  trackById(_: number, e: EstabelecimentoResponse): number {
    return e.id;
  }

  fecharModal(): void {
    this.fechar.emit();
  }
}

