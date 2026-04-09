import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api'; // השירות של PrimeNG
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const messageService = inject(MessageService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      // חילוץ ההודעה מה-Middleware ב-C#
      const errorMessage = err.error?.message || 'חלה שגיאה לא צפויה במערכת';

      // הדפסה מיוחדת - הודעה קופצת מעוצבת
      messageService.add({ 
        severity: 'error', 
        summary: 'שגיאה בשרת', 
        detail: errorMessage, 
        life: 5000 // ההודעה תיעלם אחרי 5 שניות
      });

      // אנחנו עדיין מחזירים את השגיאה כדי שדף ה-Login יוכל להפסיק את ה-Spinner
      return throwError(() => err);
    })
  );
};