import { Component, EventEmitter, Input, Output, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.css'],
})
export class UserMenuComponent {
  @Input() usuarioNome: string = '';
  @Input() avatarCor: string = '#2563eb';
  @Input() usuarioIniciais: string = '?';
  @Input() urlFotoPerfil: string | null = null;

  @Output() openEstabelecimentoModal = new EventEmitter<void>();
  @Output() editEstabelecimentos = new EventEmitter<void>();
  @Output() openPerfil = new EventEmitter<void>();
  @Output() logout = new EventEmitter<void>();

  showDropdown = false;

  constructor(private authService: AuthService, private router: Router) {}

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
  }

  closeDropdown(): void {
    this.showDropdown = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.user-menu-wrapper')) {
      this.closeDropdown();
    }
  }

  onOpenEstabelecimento(): void {
    this.openEstabelecimentoModal.emit();
    this.closeDropdown();
  }

  onEditEstabelecimentos(): void {
    this.editEstabelecimentos.emit();
    this.closeDropdown();
  }

  onOpenPerfil(): void {
    this.openPerfil.emit();
    this.closeDropdown();
  }

  onLogout(): void {
    this.authService.logout();
    this.logout.emit();
    this.closeDropdown();
    this.router.navigate(['/login']);
  }
}
