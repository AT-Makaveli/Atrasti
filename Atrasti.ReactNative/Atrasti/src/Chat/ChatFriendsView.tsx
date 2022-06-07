import React from "react";
import {
    FlatList,
    Image,
    ListRenderItemInfo,
    StyleSheet,
    Text,
    TouchableOpacity,
    View
} from "react-native";
import { getChatFriends } from "../Api/ChatAPI";
import { ChatFriend_Res } from "../Api/Models/Responds/ChatFriend_Res";
import { getLogoImage } from "../Api/APIData";
import { NavigationScreenProp } from "react-navigation";
import NavigationEvent, { fromNavigationEvent } from "../utils/NavigationEvent";
import ChatFriendEntry from "./ChatFriendEntry";

interface ChatFriendsProps {
    navigation: NavigationScreenProp<any, any>
}

interface ChatFriendsState {
    friends: ChatFriend_Res[]
}

export default class ChatFriendsView extends React.Component<ChatFriendsProps, ChatFriendsState> {

    private _onChatViewBeingShown: NavigationEvent | null = null;

    constructor(props: any) {
        super(props);

        this.state = {
            friends: []
        }
    }

    componentDidMount() {
        const event = this.props.navigation.addListener('focus', this.onRequestChats.bind(this));
        this._onChatViewBeingShown = fromNavigationEvent(event);
    }

    componentWillUnmount() {
        this._onChatViewBeingShown?.remove();
    }

    onRequestChats() {
        getChatFriends()
            .then(result => {
                this.setState({
                    friends: result.friends
                });
            })
            .catch(errors => {
                //TODO: Handle errors.
            });
    }

    renderChatUser(chatUser: ListRenderItemInfo<ChatFriendEntry>) {
        return (
            <TouchableOpacity key={chatUser.index} style={{
                flexDirection: 'row',
                alignItems: 'center',
                padding: 5
            }} onPressOut={() => {
                chatUser.item.click();
            }}>
                <Image source={{
                    uri: getLogoImage(chatUser.item.chatFriend.friendLogo)
                }} style={{
                    width: 50,
                    height: 50,
                    borderRadius: 50,
                    justifyContent: "flex-start"
                }}/>
                <Text style={[Styles.item, {
                    justifyContent: "center"
                }]}>{chatUser.item.chatFriend.friendCompany}</Text>

            </TouchableOpacity>)
    }

    render() {
        return (
            <View>
                <View style={Styles.mainView}>
                    {}
                    {this.state.friends.length > 0 ?
                        <FlatList
                            data={this.state.friends.map(value => {
                                return new ChatFriendEntry(value, this.onClickChatUser.bind(this));
                            })}
                            keyExtractor={(item: any, index: number) => index.toString()}
                            renderItem={this.renderChatUser}
                        />
                        :
                        <Text>You have no chats yet, start connecting with others!</Text>
                    }
                </View>
            </View>
        )
    }

    onClickChatUser(chatUser: ChatFriend_Res) {
        this.props.navigation.navigate('UserChat', {
            chatUser: chatUser
        });
    }
}

const Styles = StyleSheet.create({
    mainView: {
        backgroundColor: '#000000',
        display: "flex",
        minHeight: "100%",
        flexDirection: "column"
    },
    input: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        color: '#FFFFFF',
        padding: 10
    },
    item: {
        padding: 10,
        fontSize: 18,
        height: 44,
        color: '#FFFFFF'
    },
});
