import * as React from "react";
import { NavigationScreenProp } from "react-navigation";
import { GestureResponderEvent, StyleSheet, Text, TextInput, View } from "react-native";
import AtrastiButton from "../../../Shared/AtrastiButton";
import Icon from "react-native-vector-icons/FontAwesome";
import {
    EMAIL_EMPTY,
    EMAIL_IN_USE,
    EMAIL_INVALID,
    validateEmail
} from "../../../Api/RegisterAPI";
import RegisterModel from "../RegisterModel";

interface EmailRegisterProps {
    navigation: NavigationScreenProp<any, any>
}

interface EmailRegisterState {
    email: string,
    error: string | null
}

export default class EmailScreen extends React.Component<EmailRegisterProps, EmailRegisterState> {
    constructor(props: EmailRegisterProps) {
        super(props);

        this.state = {
            email: '',
            error: null
        }

        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()} style={{
                    marginLeft: 10
                }} />
            )
        });
    }

    render() {
        return (
            <View>
                <View style={Styles.mainView}>
                    <View style={Styles.content}>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Email'}
                            onChangeText={this.onEmailChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            textContentType={"emailAddress"}
                        />
                        {this.state.error !== null ? <Text style={{
                            color: '#EF5350',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 16
                        }}>{this.state.error}</Text> : null}
                        <Text style={{
                            color: '#FFFFFF',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 12
                        }}>You'll need to confirm this later</Text>
                        <AtrastiButton title={'Continue'} onClick={this.onSubmit.bind(this)}
                                       style={Styles.continueButton} textStyle={Styles.continueButtonText}/>
                    </View>
                </View>
            </View>
        );
    }

    async onSubmit(event: GestureResponderEvent) {
        if(this.state.email === null || this.state.email.length === 0) {
            this.setState({
                error: 'Email is required!'
            });
        } else {
            validateEmail(this.state.email).then(() => {
                RegisterModel.email = this.state.email;
                this.props.navigation.navigate('UserInfoScreen');
            }).catch(errors => {
                for (const error of errors) {
                    switch (error.title) {
                        case EMAIL_EMPTY:
                            this.setState({
                                error: error.message
                            })
                            break;
                        case EMAIL_INVALID:
                            this.setState({
                                error: error.message
                            })
                            break;
                        case EMAIL_IN_USE:
                            this.setState({
                                error: error.message
                            })
                            break;
                    }
                }
            });
        }
    }

    onEmailChange(text: string) {
        this.setState({
            email: text
        })
    }
}


const Styles = StyleSheet.create({
    mainView: {
        backgroundColor: '#000000',
        display: "flex",
        minHeight: "100%",
        flexDirection: "column"
    },
    content: {
        padding: 20
    },
    continueButton: {
        backgroundColor: 'transparent',
        marginTop: 10,
        borderRadius: 50,
        borderColor: '#29B6F6',
        borderWidth: 2
    },
    continueButtonText: {
        textAlign: "center",
        color: "#FFFFFF",
        padding: 10
    },
    input: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        borderRadius: 50,
        color: '#FFFFFF',
        padding: 10
    }
})
