export interface AtrastiErrorMessage {
    message: string,
    title: string
}

export interface AtrastiError {
    errors: AtrastiErrorMessage[],
    status: number
}

export function getErrors(error: AtrastiError): AtrastiErrorMessage[] {
    return error.errors;
}
