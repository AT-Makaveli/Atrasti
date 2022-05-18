import * as React from "react";
import { NavigationScreenProp } from "react-navigation";
import { StyleSheet, Text, TextInput, View } from "react-native";
import AtrastiButton from "../../../Shared/AtrastiButton";
import Icon from "react-native-vector-icons/FontAwesome";
import { COMPANY_EMPTY, COMPANY_IN_USE, validateCompany } from "../../../Api/RegisterAPI";
import RegisterModel from "../RegisterModel";

export interface CompanyRegisterProps {
    navigation: NavigationScreenProp<any, any>
}

export interface CompanyRegisterState {
    companyName: string,
    error: string | null
}

export default class CompanyScreen extends React.Component<CompanyRegisterProps, CompanyRegisterState> {

    private _atrastiButton: AtrastiButton | null;

    constructor(props: CompanyRegisterProps) {
        super(props);

        this.state = {
            companyName: '',
            error: null
        }

        this._atrastiButton = null;

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
                            placeholder={'Company Name'}
                            onChangeText={this.onCompanyNameChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            onSubmitEditing={() => {
                                this._atrastiButton?.click();
                            }}
                        />
                        {this.state.error !== null ? <Text style={{
                            color: '#EF5350',
                            paddingLeft: 10,
                            paddingTop: 4,
                            fontSize: 16
                        }}>{this.state.error}</Text> : null}
                        <AtrastiButton title={'Continue'} onClick={async (event) => {
                            if(this.state.companyName.length === 0) {
                                this.setState({
                                    error: 'Please enter company name!'
                                });
                                return;
                            }

                            validateCompany(this.state.companyName).then(() => {
                                RegisterModel.company = this.state.companyName;
                                this.props.navigation.navigate('CreatePasswordScreen');
                            }).catch(errors => {
                                for (const error of errors) {
                                    switch (error.title) {
                                        case COMPANY_EMPTY:
                                            this.setState({
                                                error: error.message
                                            })
                                            break;
                                        case COMPANY_IN_USE:
                                            this.setState({
                                                error: error.message
                                            })
                                            break;
                                    }
                                }
                            });

                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText} ref={(ref) => {
                            this._atrastiButton = ref;
                        }}/>
                    </View>
                </View>
            </View>
        );
    }

    onCompanyNameChange(text: string) {
        this.setState({
            companyName: text
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
