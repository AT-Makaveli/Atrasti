

export interface ChatMessage_Res {
    id: number;
    senderId: number;
    chatMessage: string;
    created: Date;
    fromMe: boolean;
    author: string;
    hasBeenRead: boolean;
}