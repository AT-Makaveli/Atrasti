import * as React from "react";
import {
    GestureResponderEvent,
    StyleProp,
    Text,
    TextStyle,
    TouchableHighlight,
    View,
    ViewStyle
} from "react-native";

interface RegisterButtonProps {
    title: string,
    onClick: (event: GestureResponderEvent) => void,
    style: StyleProp<ViewStyle>,
    textStyle: StyleProp<TextStyle>
}

export default class AtrastiButton extends React.Component<RegisterButtonProps> {

    private _touchable: TouchableHighlight | null;

    constructor(props: RegisterButtonProps) {
        super(props);

        this._touchable = null;
    }

    render() {
        return (
            <TouchableHighlight onPress={this.props.onClick} ref={(ref) => {
                this._touchable = ref;
            }}>
                <View style={this.props.style}>
                    <Text style={this.props.textStyle}>{this.props.title}</Text>
                </View>
            </TouchableHighlight>
        )
    }

    click() {
        // @ts-ignore
        this.props.onClick(null);
    }
}
