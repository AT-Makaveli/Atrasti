import { StyleSheet, Text, View } from "react-native";
import React from "react";
import { getWindowDimensions } from "../utils/WindowUtils";

interface ChatBubbleProps {
    message: string,
    fromMe: boolean,
    author: string | null,
    date: string
}

export default class ChatBubble extends React.Component<ChatBubbleProps, any> {
    constructor(props: ChatBubbleProps) {
        super(props);
    }

    render() {
        return (
            <View style={{
                width: getWindowDimensions().width - 20,
                marginLeft: 10,
                marginTop: 10
            }}>
                <View style={this.props.fromMe ? Styles.fromMe : Styles.fromUser}>
                    {this.props.fromMe ? null : (
                        <Text style={{
                            color: '#FFFFFF',
                            fontSize: 12,
                            fontWeight: "bold"
                        }}>{this.props.author}</Text>
                    )}
                    <Text style={{
                        color: '#FFFFFF'
                    }}>{this.props.message}</Text>
                </View>
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    fromMe: {
        maxWidth: '75%',
        alignSelf: "flex-end",
        padding: 5,
        borderRadius: 7.5,
        borderWidth: 2,
        borderColor: '#29B6F6'
    },
    fromUser: {
        backgroundColor: '#29B6F6',
        maxWidth: '75%',
        alignSelf: "flex-start",
        padding: 5,
        borderRadius: 7.5,
    }
});
