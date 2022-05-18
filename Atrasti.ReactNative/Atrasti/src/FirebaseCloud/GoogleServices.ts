export interface GoogleServices {
    project_info: GoogleProjectInfo
    client: GoogleClient[]
}

export interface GoogleProjectInfo {
    project_number: string,
    project_id: string,
    storage_bucket: string
}

export interface GoogleClient {
    client_info: GoogleClientInfo
}

export interface GoogleClientInfo {
    mobilesdk_app_id: string
}
