import * as React from "react";
import { NavigationScreenProp } from "react-navigation";
import { Button, StyleSheet, Text, TextInput, View } from "react-native";
import AtrastiButton from "../../../Shared/AtrastiButton";
import Icon from "react-native-vector-icons/FontAwesome";
import RegisterModel from "../RegisterModel";
import { Ref } from "react";

interface UserInfoRegisterProps {
    navigation: NavigationScreenProp<any, any>
}

interface UserInfoRegisterState {
    firstName: string,
    lastName: string,
    error: string | null
}

export default class UserInfoScreen extends React.Component<UserInfoRegisterProps, UserInfoRegisterState> {

    private _secondTextInput: TextInput | null;
    private _submitButton: AtrastiButton | null;

    constructor(props: UserInfoRegisterProps) {
        super(props);

        this.state = {
            firstName: '',
            lastName: '',
            error: null
        }

        this._secondTextInput = null;
        this._submitButton = null;

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
                            placeholder={'First Name'}
                            onChangeText={this.onFirstNameChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            onSubmitEditing={() => {
                                this._secondTextInput?.focus();
                            }}
                        />
                        <TextInput
                            style={[Styles.input, {
                                marginTop: 10
                            }]}
                            placeholder={'Last Name'}
                            onChangeText={this.onLastNameChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            ref={(ref) => {
                                this._secondTextInput = ref;
                            }}
                            onSubmitEditing={() => {
                                this._submitButton?.click();
                            }}
                        />
                        {this.state.error !== null ? <Text style={{
                            color: '#EF5350',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 16
                        }}>{this.state.error}</Text> : null}
                        <AtrastiButton title={'Continue'} onClick={(event) => {
                            if(this.state.firstName.length === 0) {
                                this.setState({
                                    error: 'Please enter first name!'
                                });
                                return;
                            }

                            if(this.state.lastName.length === 0) {
                                this.setState({
                                    error: 'Please enter last name!'
                                });
                                return;
                            }

                            RegisterModel.firstName = this.state.firstName;
                            RegisterModel.lastName = this.state.lastName;

                            this.props.navigation.navigate('ChooseTypeScreen');
                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText} ref={(ref) => {
                            this._submitButton = ref;
                        }}/>
                    </View>
                </View>
            </View>
        );
    }

    onFirstNameChange(text: string) {
        this.setState({
            firstName: text
        })
    }

    onLastNameChange(text: string) {
        this.setState({
            lastName: text
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
