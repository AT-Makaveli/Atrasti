import { ChatFriend_Res } from "../Api/Models/Responds/ChatFriend_Res";

export default class ChatFriendEntry {
    private readonly _chatFriend: ChatFriend_Res;
    private readonly _onClick: (chatFriend: ChatFriend_Res) => void;

    constructor(chatFriend: ChatFriend_Res, onClick: (chatFriend: ChatFriend_Res) => void) {
        this._chatFriend = chatFriend;
        this._onClick = onClick;
    }

    get chatFriend(): ChatFriend_Res {
        return this._chatFriend;
    }

    click(): void {
        this._onClick(this.chatFriend);
    }
}
