import React from "react";
import { CardStyleInterpolators, createStackNavigator } from "@react-navigation/stack";
import ChatView from "./ChatView";
import { HeaderTitleProps } from "@react-navigation/elements";
import { Text } from "react-native";
import ChatFriendsView from "./ChatFriendsView";

const Stack = createStackNavigator();

export default class ChatStack extends React.Component<any, any> {

    render() {
        return (
            <Stack.Navigator initialRouteName={"ChatFriends"} screenOptions={{
                headerShown: false,
                gestureDirection: "horizontal",
                cardStyleInterpolator: CardStyleInterpolators.forHorizontalIOS,
            }}>
                <Stack.Screen name="ChatFriends" component={ChatFriendsView}/>
                <Stack.Screen name="UserChat" component={ChatView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Chat user</Text>
                    }
                }} />
            </Stack.Navigator>
        );
    }
}
