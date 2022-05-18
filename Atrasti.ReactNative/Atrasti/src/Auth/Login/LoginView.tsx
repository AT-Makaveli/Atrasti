import * as React from "react";
import { DeviceEventEmitter, Keyboard, StyleSheet, Text, TextInput, View } from "react-native";
import AtrastiButton from "../../Shared/AtrastiButton";
import { NavigationScreenProp } from 'react-navigation';
import Icon from "react-native-vector-icons/FontAwesome";
import AuthRequestEvent from "../../utils/Events/Auth/AuthRequestEvent";
import { AUTH_ALL_FIELDS, INVALID_EMAIL_OR_PASSWORD, login, LOGIN_FAILED, LOGIN_SUCCESS } from "../../Api/LoginAPI";
import { AtrastiErrorMessage } from "../../utils/ErrorHelpers";

interface LoginProps {
    navigation: NavigationScreenProp<any, any>
}

interface LoginState {
    email: string,
    password: string,
    error: string | null
}

export default class LoginView extends React.Component<LoginProps, LoginState> {

    constructor(props: LoginProps) {
        super(props);

        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()} style={{
                    marginLeft: 10
                }} />
            )
        });

        this.state = {
            email: '',
            password: '',
            error: null
        }
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
                        />
                        {this.state.error !== null ? <Text style={{
                            color: '#EF5350',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 16
                        }}>{this.state.error}</Text> : null}
                        <TextInput
                            style={[Styles.input, {
                                marginTop: 10
                            }]}
                            placeholder={'Password'}
                            onChangeText={this.onPasswordChange.bind(this)}
                            secureTextEntry={true}
                            placeholderTextColor={'#FEFEFE'}
                        />
                        <AtrastiButton title={'Login'} onClick={this.onLogin.bind(this)} style={Styles.continueButton}
                                       textStyle={Styles.continueButtonText}/>
                        <AtrastiButton title={'Forgot password'} onClick={this.onForgotPassword.bind(this)} style={Styles.continueButton}
                                       textStyle={Styles.continueButtonText}/>
                    </View>
                </View>
            </View>
        )
    }

    onLogin() {
        Keyboard.dismiss();
        login(this.state.email, this.state.password).then(result => {
            if(result.code === LOGIN_SUCCESS) {
                DeviceEventEmitter.emit(AuthRequestEvent.EVENT_TYPE);
            } else {
                console.log('received invalid response code from server!');
            }
        }).catch((errors: AtrastiErrorMessage[]) => {
            for (const error of errors) {
                switch (error.title) {
                    case INVALID_EMAIL_OR_PASSWORD:
                        this.setState({
                            error: error.message
                        })
                        break;
                    case AUTH_ALL_FIELDS:
                        break;
                }
            }
        });
    }

    onForgotPassword() {
        this.props.navigation.navigate('ForgotPassword');
    }

    onEmailChange(text: string) {
        this.setState({
            email: text
        })
    }

    onPasswordChange(text: string) {
        this.setState({
            password: text
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
    contentHeader: {
        flexGrow: 1,
        marginTop: "auto",
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
