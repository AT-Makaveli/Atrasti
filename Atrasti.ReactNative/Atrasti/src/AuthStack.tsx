import { CardStyleInterpolators, createStackNavigator } from "@react-navigation/stack";
import MainAuthView from "./Auth/MainAuthView";
import React from "react";
import LoginView from "./Auth/Login/LoginView";
import RegisterStack from "./Auth/Register/RegisterStack";
import { HeaderTitleProps } from "@react-navigation/elements";
import { Text } from "react-native";
import ForgotPasswordView from "./Auth/Login/ForgotPasswordView";

const Stack = createStackNavigator();


export default class AuthStack extends React.Component {

    constructor(props: any) {
        super(props);
    }

    render() {
        return (
            <Stack.Navigator initialRouteName={"Home"} screenOptions={{
                headerShown: false,
                gestureDirection: "horizontal",
                cardStyleInterpolator: CardStyleInterpolators.forHorizontalIOS
            }}>
                <Stack.Screen name="Home" component={MainAuthView}/>
                <Stack.Screen name="Login" component={LoginView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Login</Text>
                    }
                }} />
                <Stack.Screen name={"ForgotPassword"} component={ForgotPasswordView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Forgot password</Text>
                    }
                }} />
                <Stack.Screen name="Register" component={RegisterStack} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: ''
                }}/>
            </Stack.Navigator>
        );
    }
}
