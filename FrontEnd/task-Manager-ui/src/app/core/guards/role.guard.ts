import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const roleGuard: (allowedRoles: string[]) => CanActivateFn = (allowedRoles) => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isLoggedIn()) {
      router.navigate(['/login']);
      return false;
    }

    const role = authService.getRole();
    if (allowedRoles.includes(role)) {
      return true;
    }

    // Redirect based on role
    if (role === 'Admin') {
      router.navigate(['/admin']);
    } else {
      router.navigate(['/projects']);
    }
    return false;
  };
};
