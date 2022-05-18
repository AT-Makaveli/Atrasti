import React from "react";
import { DeviceEventEmitter, EmitterSubscription, Keyboard, ScrollView, Text, TextInput, View } from "react-native";
import { NavigationRoute, NavigationScreenProp } from "react-navigation";
import Icon from "react-native-vector-icons/FontAwesome";
import { HeaderTitleProps } from "@react-navigation/elements";

import * as IonIcons from "react-native-vector-icons/Ionicons";
import ChatBubble from "./ChatBubble";
import { getChatMessages, sendChatMessage } from "../Api/ChatAPI";
import { ChatMessage_Res } from "../Api/Models/Responds/ChatMessage_Res";
import ChatReceiveEvent from "../utils/Events/Chat/ChatReceiveEvent";

export interface ChatViewProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
}

export interface ChatViewState {
    textInput: string;
    chatId: number;
    friendId: number;

    chatMessages: ChatMessage_Res[]
}

export default class ChatView extends React.Component<ChatViewProps, ChatViewState> {

    private _scrollView: ScrollView | null = null;
    private receiveMessageSubscription: EmitterSubscription | null = null;
    private _keyboardShowListener: EmitterSubscription | null = null;
    private _keyboardHideListener: EmitterSubscription | null = null;

    constructor(props: ChatViewProps) {
        super(props);
        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()} style={{
                    marginLeft: 10
                }}/>
            ),
            headerTitle: (props: HeaderTitleProps): any => {
                return <Text style={{
                    color: '#FFFFFF',
                    fontSize: 16,
                    fontWeight: "bold"
                }}>{this.props.route.params?.chatUser.friendCompany}</Text>
            }
        });

        this.state = {
            textInput: '',
            chatId: this.props.route.params?.chatUser.chatId,
            friendId: this.props.route.params?.chatUser.friendId,
            chatMessages: []
        }
    }

    componentDidMount() {
        this.receiveMessageSubscription = DeviceEventEmitter.addListener(ChatReceiveEvent.EVENT_TYPE, this.onReceiveChatMessage.bind(this));
        this._keyboardShowListener = Keyboard.addListener("keyboardDidShow", () => {
            console.log('keyboard showed!');
            this._scrollView?.scrollToEnd({
                animated: false
            })
        })
        this._keyboardHideListener = Keyboard.addListener("keyboardDidHide", () => {
            console.log('keyboard Hidden!');
            this._scrollView?.scrollToEnd({
                animated: false
            })
        })

        getChatMessages(this.state.chatId).then(response => {
            this.setState({
                chatMessages: response.messages
            })
        }).catch(errors => {
            console.log(errors);
        })
    }

    onReceiveChatMessage(data: ChatReceiveEvent) {
        const messages = this.state.chatMessages;
        let newMessage = {
            chatMessage: data.chatMessage,
            fromMe: data.fromMe,
            created: data.created,
            author: data.chatAuthor
        } as ChatMessage_Res;
        messages.push(newMessage);
        this.setState({
            chatMessages: messages
        })
    }

    componentWillUnmount() {
        this.receiveMessageSubscription?.remove();
        this._keyboardShowListener?.remove();
        this._keyboardHideListener?.remove();
    }

    onTextInputChange(text: string) {
        this.setState({
            textInput: text
        });
    }

    onSendChatPressed() {
        const text = this.state.textInput;
        if(text.length === 0) {
            return;
        }

        sendChatMessage(this.state.chatId, this.state.friendId, text).then(response => {
            const messages = this.state.chatMessages;
            messages.push(response);
            messages.sort(x => x.id);
            this.setState({
                chatMessages: messages
            })
        }).catch(error => {
            console.log(error);
        })
    }

    render() {
        return (
            <View style={{
                backgroundColor: '#000000',
                flex: 1
            }}>
                <ScrollView ref={ref => this._scrollView = ref} onContentSizeChange={() => this._scrollView?.scrollToEnd({animated: false})} style={{
                    flexGrow: 1
                }}>
                    {this.state.chatMessages.map((value, index) => {
                        return <ChatBubble key={index} message={value.chatMessage} fromMe={value.fromMe}
                                           author={value.author} date={new Date(value.created).toTimeString()}/>
                    })}
                </ScrollView>
                <View style={{
                    flexDirection: "row"
                }}>
                    <IonIcons.default name={'attach-outline'} size={26} color={'#FFFFFF'} onPress={() => {
                    }} style={{
                        width: 50,
                        height: 50,
                        backgroundColor: '#0baf9d',
                    }}/>
                    <TextInput multiline={true} placeholder={'Start writing...'} style={{
                        backgroundColor: '#FF0000',
                        flex: 1,
                    }} value={this.state.textInput} onChangeText={this.onTextInputChange.bind(this)}/>
                    <IonIcons.default name={'send-outline'} size={24} color={'#FFFFFF'}
                                      onPress={this.onSendChatPressed.bind(this)} style={{
                        padding: 10
                    }}/>
                </View>
            </View>
        );
    }
}
