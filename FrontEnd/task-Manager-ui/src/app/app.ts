import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive, Router, NavigationEnd } from '@angular/router';
import { AuthService } from './core/services/auth';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  showSidebar = false;
  isAdmin = false;

  constructor(
    public authService: AuthService,
    private router: Router
  ) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event) => {
      const navEvent = event as NavigationEnd;
      const authRoutes = ['/login', '/register'];
      const isAuthPage = authRoutes.some(route => navEvent.urlAfterRedirects.startsWith(route));

      this.showSidebar = this.authService.isLoggedIn() && !isAuthPage;
      this.isAdmin = this.authService.getRole() === 'Admin';
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
