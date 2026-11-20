import { HttpErrorResponse } from '@angular/common/http';

/** Turns an API error (ProblemDetails or network failure) into a message for the user. */
export function describeHttpError(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return 'Cannot reach the practice service. Make sure it is running.';
    }
    return error.error?.detail ?? error.error?.title ?? error.message;
  }
  return error instanceof Error ? error.message : String(error);
}
