import * as React from "react";
import { Keyboard, StyleSheet, Text, TextInput, View } from "react-native";
import AtrastiButton from "../../Shared/AtrastiButton";
import { NavigationScreenProp } from 'react-navigation';
import Icon from "react-native-vector-icons/FontAwesome";
import { forgotPassword } from "../../Api/LoginAPI";
import { AtrastiError, AtrastiErrorMessage } from "../../utils/ErrorHelpers";

interface ForgotPasswordProps {
    navigation: NavigationScreenProp<any, any>
}

interface ForgotPasswordState {
    email: string,
    error: string,
    success: boolean
}

export default class ForgotPasswordView extends React.Component<ForgotPasswordProps, ForgotPasswordState> {

    constructor(props: ForgotPasswordProps) {
        super(props);

        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()} style={{
                    marginLeft: 10
                }}/>
            )
        });

        this.state = {
            email: '',
            error: '',
            success: false
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
                        {this.state.success && <Text style={{
                            color: '#4CAF50',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 16
                        }}>If this email matches any records an email will be sent to you. Please wait a few minutes for
                            the verification link.</Text>}
                        {this.state.error.length > 0 && <Text style={{
                            color: '#EF5350',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 16
                        }}>{this.state.error}</Text>}
                        <AtrastiButton title={'Send password reset email'} onClick={this.onResetPassword.bind(this)} style={Styles.continueButton}
                                       textStyle={Styles.continueButtonText}/>
                    </View>
                </View>
            </View>
        )
    }

    onResetPassword() {
        Keyboard.dismiss();
        forgotPassword(this.state.email)
            .then(result => {
                console.log(result);
                if (result.status) {
                    this.setState({
                        success: result.status,
                        error: ''
                    })
                }
            })
            .catch((errors: AtrastiErrorMessage[]) => {
                let errorString = '';
                for (const error of errors) {
                    errorString += error.message + '\n';
                }

                this.setState({
                    error: errorString
                })
            });
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
