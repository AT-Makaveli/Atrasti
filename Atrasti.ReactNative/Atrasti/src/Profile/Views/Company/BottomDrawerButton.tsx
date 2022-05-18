import React from "react";
import { StyleSheet, Text, TouchableOpacity, TouchableWithoutFeedback, View } from "react-native";
import Icon from "react-native-vector-icons/Ionicons";

export interface BottomDrawerButtonProps {
    onPress: () => void,
    title: string,
    icon: any
}

export interface BottomDrawerButtonState {
    pressed: boolean
}

export default class BottomDrawerButton extends React.Component<BottomDrawerButtonProps, BottomDrawerButtonState> {

    constructor(props: BottomDrawerButtonProps) {
        super(props);

        this.state = {
            pressed: false
        }
    }

    render() {
        return (
            <TouchableWithoutFeedback onPressIn={() => {
                this.setState({
                    pressed: true
                })
            }} onPressOut={() => {
                this.setState({
                    pressed: false
                })
            }} onPress={this.props.onPress}>
                <View style={{
                    backgroundColor: this.state.pressed ? '#52595f' : '#343a40',
                    flexDirection: "row",
                    padding: 10,
                    paddingLeft: 0
                }}>
                    {this.props.icon}
                    <Text style={styles.buttonText}>{this.props.title}</Text>
                </View>
            </TouchableWithoutFeedback>
        );
    }
}

const styles = StyleSheet.create({
    button: {

    },
    buttonText: {
        fontSize: 20,
        fontWeight: "300",
        color: '#FFFFFF'
    }
});
