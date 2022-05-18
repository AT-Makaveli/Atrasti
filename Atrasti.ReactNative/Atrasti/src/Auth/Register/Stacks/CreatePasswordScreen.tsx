import * as React from "react";
import { NavigationActions, NavigationScreenProp } from "react-navigation";
import { Text, StyleSheet, TextInput, View } from "react-native";
import AtrastiButton from "../../../Shared/AtrastiButton";
import { Icon } from "react-native-elements";
import { register } from "../../../Api/RegisterAPI";
import RegisterModel from "../RegisterModel";

interface PasswordRegisterProps {
    navigation: NavigationScreenProp<any, any>
}

interface PasswordRegisterState {
    password: string,
    confirmPassword: string,
    errors: string[],

    lowerCase: boolean,
    upperCase: boolean,
    digit: boolean,
    special: boolean,
    greaterEight: boolean
}

export default class PasswordScreen extends React.Component<PasswordRegisterProps, PasswordRegisterState> {

    private _secondPasswordInput: TextInput | null;
    private _submitButton: AtrastiButton | null;

    constructor(props: PasswordRegisterProps) {
        super(props);

        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()}/>
            )
        });

        this.state = {
            password: '',
            confirmPassword: '',
            errors: [],

            lowerCase: false,
            upperCase: false,
            digit: false,
            special: false,
            greaterEight: false
        }

        this._secondPasswordInput = null;
        this._submitButton = null;
    }

    render() {
        return (
            <View>
                <View style={Styles.mainView}>
                    <View style={Styles.content}>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Password'}
                            onChangeText={this.onPasswordChange.bind(this)}
                            secureTextEntry={(this.state.password.length > 0)}
                            placeholderTextColor={'#FEFEFE'}
                            onSubmitEditing={() => {
                                this._secondPasswordInput?.focus();
                            }}
                        />
                        {this.state.errors.length > 0 ? this.state.errors.map(value => {
                            return <Text style={{
                                color: '#EF5350',
                                paddingLeft: 10,
                                paddingTop: 4,
                                fontSize: 16
                            }}>{value}</Text>
                        }) : null}
                        <TextInput
                            style={[Styles.input, {
                                marginTop: 10
                            }]}
                            placeholder={'Confirm Password'}
                            onChangeText={this.onConfirmPasswordChange.bind(this)}
                            secureTextEntry={(this.state.confirmPassword.length > 0)}
                            placeholderTextColor={'#FEFEFE'}
                            ref={(ref) => {
                                this._secondPasswordInput = ref;
                            }}
                            onSubmitEditing={() => {
                                this._submitButton?.click();
                            }}
                        />
                        <View style={{
                            display: "flex",
                            flexDirection: "row"
                        }}>
                            <Text style={[
                                {
                                    backgroundColor: this.state.lowerCase ? '#4CAF50' : '#EF5350',
                                }, Styles.matchBoxes
                            ]}>
                                a-z
                            </Text>
                            <Text style={[
                                {
                                    backgroundColor: this.state.upperCase ? '#4CAF50' : '#EF5350',
                                }, Styles.matchBoxes
                            ]}>
                                A-Z
                            </Text>
                            <Text style={[
                                {
                                    backgroundColor: this.state.digit ? '#4CAF50' : '#EF5350',
                                }, Styles.matchBoxes
                            ]}>
                                0-9
                            </Text>
                            <Text style={[
                                {
                                    backgroundColor: this.state.special ? '#4CAF50' : '#EF5350',
                                }, Styles.matchBoxes
                            ]}>
                                !-#
                            </Text>
                            <Text style={[
                                {
                                    backgroundColor: this.state.greaterEight ? '#4CAF50' : '#EF5350',
                                }, Styles.matchBoxes
                            ]}>
                                ≥8
                            </Text>
                        </View>
                        <AtrastiButton title={'Register'} onClick={() => {
                            this.onSubmit();
                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText} ref={(ref) => {
                            this._submitButton = ref;
                        }}/>
                    </View>
                </View>
            </View>
        );
    }

    onSubmit() {
        const errors = [];

        if(this.state.password !== this.state.confirmPassword) {
            errors.push('Password do not match.');
        }

        if(!this.state.password.match(new RegExp("^(?=.*[a-z])"))) {
            errors.push('Password must have lower-case letter.');
        }

        if(!this.state.password.match(new RegExp("^(?=.*[A-Z])"))) {
            errors.push('Password must have upper-case letter.');
        }

        if(!this.state.password.match(new RegExp("^(?=.*[0-9])"))) {
            errors.push('Password must have a number.');
        }

        if(!this.state.password.match(new RegExp("^(?=.*[!\"#$%&'()*+,./;:=?_@>-])"))) {
            errors.push('Password must contain special characters.');
        }

        if(!(this.state.password.length >= 8)) {
            errors.push('Password must be longer than 7 characters.');
        }

        if(errors.length > 0) {
            this.setState({
                errors: errors
            });

            return;
        }

        register(RegisterModel.email, RegisterModel.firstName, RegisterModel.lastName, RegisterModel.company, this.state.password)
            .then(result => {
                this.props.navigation.navigate(  'Login',
                    {},
                    NavigationActions.navigate({
                        routeName: 'Login'
                    }));
                //this.props.navigation.navigate('LoginView');
            }).catch(errors => {

        });
    }

    onPasswordChange(text: string) {
        let lowerCase = false;
        let upperCase = false;
        let digit = false;
        let special = false;
        let greaterEight = false;
        if(text.match(new RegExp("^(?=.*[a-z])"))) {
            lowerCase = true;
        }

        if(text.match(new RegExp("^(?=.*[A-Z])"))) {
            upperCase = true;
        }

        if(text.match(new RegExp("^(?=.*[0-9])"))) {
            digit = true;
        }

        if(text.match(new RegExp("^(?=.*[!\"#$%&'()*+,./;:=?_@>-])"))) {
            special = true;
        }

        if(text.length >= 8) {
            greaterEight = true;
        }

        this.setState({
            password: text,
            lowerCase: lowerCase,
            upperCase: upperCase,
            digit: digit,
            special: special,
            greaterEight: greaterEight
        })
    }

    onConfirmPasswordChange(text: string) {
        this.setState({
            confirmPassword: text
        })
    }
}


const Styles = StyleSheet.create({
    matchBoxes: {
        color: '#FFFFFF',
        padding: 4,
        margin: 4,
        marginTop: 10,
        flexGrow: 5,
        textAlign: "center",
        borderRadius: 10
    },
    centeredView: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
        marginTop: 22
    },
    modalView: {
        margin: 20,
        backgroundColor: "white",
        borderRadius: 20,
        padding: 35,
        alignItems: "center",
        shadowColor: "#000",
        shadowOffset: {
            width: 0,
            height: 2
        },
        shadowOpacity: 0.25,
        shadowRadius: 4,
        elevation: 5
    },
    button: {
        borderRadius: 20,
        padding: 10,
        elevation: 2
    },
    buttonOpen: {
        backgroundColor: "#F194FF",
    },
    buttonClose: {
        backgroundColor: "#2196F3",
    },
    textStyle: {
        color: "white",
        fontWeight: "bold",
        textAlign: "center"
    },
    modalText: {
        marginBottom: 15,
        textAlign: "center"
    },

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
        padding: 10,
    }
})
