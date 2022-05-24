import * as React from "react";
import { NavigationScreenProp } from "react-navigation";
import { StyleSheet, Text, View } from "react-native";
import AtrastiButton from "../../../Shared/AtrastiButton";
import Icon from "react-native-vector-icons/FontAwesome";
import RegisterModel from "../RegisterModel";
import { UserType_Mod } from "../../../Api/Models/DbModels/UserType_Mod";

export interface CompanyRegisterProps {
    navigation: NavigationScreenProp<any, any>
}

export default class TypeOfUserScreen extends React.Component<CompanyRegisterProps, any> {

    constructor(props: CompanyRegisterProps) {
        super(props);

        this.state = {
            companyName: '',
            error: null
        }

        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()} style={{
                    marginLeft: 10
                }}/>
            )
        });
    }

    render() {

        return (
            <View>
                <View style={Styles.mainView}>
                    <View style={Styles.content}>
                        <Text style={{
                            color: '#FFFFFF',
                            paddingLeft: 10,
                            fontSize: 24
                        }}>
                            Agent
                        </Text>
                        <Text style={{
                            color: '#FFFFFF',
                            paddingLeft: 10,
                            fontSize: 16
                        }}>
                            Are you an agent looking for new missions?
                        </Text>
                        <AtrastiButton title={'Agent'} onClick={async (event) => {
                            RegisterModel.userType = UserType_Mod.AGENT;
                            this.props.navigation.navigate('CreatePasswordScreen');
                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText}/>
                        <Text style={{
                            color: '#FFFFFF',
                            paddingLeft: 10,
                            fontSize: 24,
                            paddingTop: 10
                        }}>
                            Company
                        </Text>
                        <Text style={{
                            color: '#FFFFFF',
                            paddingLeft: 10,
                            fontSize: 16
                        }}>
                            Are you a buying/selling company?
                        </Text>
                        <AtrastiButton title={'Company'} onClick={async (event) => {
                            RegisterModel.userType = UserType_Mod.COMPANY;
                            this.props.navigation.navigate('CompanyScreen');
                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText}/>
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
