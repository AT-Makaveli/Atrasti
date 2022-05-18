import * as React from "react";
import { NavigationScreenProp } from 'react-navigation';
import { CardStyleInterpolators, createStackNavigator } from "@react-navigation/stack";
import EmailScreen from "./Stacks/EmailScreen";
import CompanyScreen from "./Stacks/CompanyScreen";
import UserInfoScreen from "./Stacks/UserInfoScreen";
import { useState } from "react";
import CreatePasswordScreen from "./Stacks/CreatePasswordScreen";
import { HeaderTitleProps } from "@react-navigation/elements";
import { Text } from "react-native";
import TypeOfUserScreen from "./Stacks/TypeOfUserScreen";

interface RegisterProps {
    navigation: NavigationScreenProp<any, any>
}

const Stack = createStackNavigator();

export default class RegisterStack extends React.Component<RegisterProps> {

    constructor(props: RegisterProps) {
        super(props);

        // @ts-ignore
        this.props.navigation.setOptions({
            headerShown: false
        });
    }

    render() {
        return (
            <Stack.Navigator initialRouteName={"EmailScreen"} screenOptions={{
                headerShown: false,
                gestureDirection: "horizontal",
                cardStyleInterpolator: CardStyleInterpolators.forHorizontalIOS,
            }}>
                <Stack.Screen name="EmailScreen" component={EmailScreen} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Choose email</Text>
                    }
                }}/>
                <Stack.Screen name="UserInfoScreen" component={UserInfoScreen} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>User details</Text>
                    }
                }}/>
                <Stack.Screen name="ChooseTypeScreen" component={TypeOfUserScreen} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Type of user</Text>
                    }
                }}/>
                <Stack.Screen name="CompanyScreen" component={CompanyScreen} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Company details</Text>
                    }
                }}/>
                <Stack.Screen name="CreatePasswordScreen" component={CreatePasswordScreen} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Password details</Text>
                    }
                }}/>
            </Stack.Navigator>
        )
    }
}
