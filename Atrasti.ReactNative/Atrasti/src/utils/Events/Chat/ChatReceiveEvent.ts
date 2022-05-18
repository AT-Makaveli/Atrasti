export default class ChatReceiveEvent {
    public static EVENT_TYPE = 'CHAT_RECEIVE_EVENT';

    public created: Date;
    public fromMe: boolean;
    public fromUserId: number;
    public chatMessage: string;
    public chatAuthor: string;

    constructor(data: any) {
        this.created = data['chat-created'];
        this.fromMe = data['chat-from-me'] === "True";
        this.fromUserId = data['chat-from-user-id'];
        this.chatMessage = data['chat-message'];
        this.chatAuthor = data['chat-author'];
    }
}
