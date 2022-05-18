import * as React from "react";
import { Image, StyleSheet, Text, View } from "react-native";
import AtrastiButton from "../Shared/AtrastiButton";
import LinearGradient from 'react-native-linear-gradient';
import { NavigationScreenProp } from "react-navigation";
import { AtrastiLogo } from "../utils/StaticImageUrls";

interface MainAuthViewProps {
    navigation: NavigationScreenProp<any,any>
}

export default class MainAuthView extends React.Component<MainAuthViewProps, any> {
    constructor(props: MainAuthViewProps) {
        super(props);
    }

    render() {
        return (
            <View>
                <View style={Styles.mainView}>
                    <View style={Styles.contentHeader}>
                        <Image source={require('../../assets/images/engineblock.jpg')} style={{
                            width: '100%',
                        }}/>
                        <LinearGradient colors={['transparent', '#000000']} start={{x: 0, y: 0.6}} end={{x: 0, y: 1}}
                                        style={Styles.linearGradient}>
                            <Image source={AtrastiLogo} style={Styles.logo}/>
                            <Text style={Styles.gradientText}>Get discovered</Text>
                            <Text style={Styles.gradientText}>Discover companies</Text>
                        </LinearGradient>
                    </View>

                    <View style={Styles.content}>
                        <AtrastiButton title={'Register'} onClick={(event) => {
                            this.props.navigation.navigate('Register');
                        }} style={Styles.setupButton} textStyle={Styles.setupButtonText}/>

                        <AtrastiButton title={'Login'} onClick={(event) => {
                            this.props.navigation.navigate('Login');
                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText}/>
                    </View>
                </View>
            </View>
        );
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
    logo: {
        alignSelf: "center",
        marginTop: "auto",
        marginBottom: 10
    },
    setupButton: {
        backgroundColor: '#29B6F6',
        marginTop: 10,
        borderRadius: 50
    },
    setupButtonText: {
        textAlign: "center",
        color: "#FFFFFF",
        padding: 10
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
    linearGradient: {
        flex: 1,
        position: "absolute",
        zIndex: 12,
        top: 0,
        bottom: 0,
        right: 0,
        left: 0
    },
    gradientText: {
        fontSize: 18,
        color: "white",
        textAlign: "center",
        fontWeight: "bold"
    }
})
