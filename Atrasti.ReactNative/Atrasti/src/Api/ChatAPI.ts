import { CHAT_GET_FRIENDS, CHAT_GET_MESSAGES, CHAT_SEND_CHAT } from "./APIData";
import { HttpServiceSingleton } from "../Services/HttpService";
import { getErrors } from "../utils/ErrorHelpers";
import { ChatFriends_Res } from "./Models/Responds/ChatFriends_Res";
import { SendChatMessage_Req } from "./Models/Requests/SendChatMessage_Req";
import { ChatMessage_Res } from "./Models/Responds/ChatMessage_Res";
import { ChatMessages_Req } from "./Models/Requests/ChatMessages_Req";
import { ChatMessages_Res } from "./Models/Responds/ChatMessages_Res";

export const USER_NOT_SET = "USER_NOT_SET";
export const USER_PROFILE_NOT_SET = "USER_PROFILE_NOT_SET";

export function getChatFriends(): Promise<ChatFriends_Res> {
    return new Promise<ChatFriends_Res>((resolve, reject) => {
        HttpServiceSingleton.get<ChatFriends_Res>(CHAT_GET_FRIENDS).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function getChatMessages(chatId: number) {
    const req = {
        chatId: chatId
    } as ChatMessages_Req;

    return new Promise<ChatMessages_Res>((resolve, reject) => {
        HttpServiceSingleton.post<ChatMessages_Res>(CHAT_GET_MESSAGES, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function sendChatMessage(chatId: number, friendId: number, message: string): Promise<ChatMessage_Res> {
    const req = {
        chatId: chatId,
        friendId: friendId,
        message: message
    } as SendChatMessage_Req;

    return new Promise<ChatMessage_Res>((resolve, reject) => {
        HttpServiceSingleton.post<ChatMessage_Res>(CHAT_SEND_CHAT, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data))
        })
    });
}
