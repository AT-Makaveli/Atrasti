import React from "react";
import HomeView from "./Home/HomeView";
import Icon from "react-native-vector-icons/Ionicons";
import { Text } from "react-native";
import SearchView from "./Search/SearchView";
import ProfileStack from "./Profile/ProfileStack";
import ChatStack from "./Chat/ChatStack";
import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";


const Tab = createBottomTabNavigator();

export default class AuthTabs extends React.Component<any, any> {
    render() {
        return (
            <Tab.Navigator id={'TabNavigator'} screenOptions={{
                headerShown: false,
                tabBarStyle: {
                    backgroundColor: '#343a40',
                    borderTopColor: '#FFFFFF',
                    borderTopWidth: 1,
                },
                tabBarHideOnKeyboard: true
            }}>
                <Tab.Screen name="Home" component={HomeView} options={{
                    tabBarIcon: ({focused}) => (
                        <Icon name={focused ? 'home' : 'home-outline'} style={{
                            fontSize: 20
                        }} color={focused ? '#ffffff' : '#939393'}/>
                    ),
                    tabBarLabel: ({focused}) => <Text style={{
                        color: focused ? '#ffffff' : '#939393'
                    }}>Home</Text>,
                }}/>
                <Tab.Screen name="Search" component={SearchView} options={{
                    tabBarIcon: ({focused}) => (
                        <Icon name={'search'} style={{
                            fontSize: 20,
                        }} color={focused ? '#ffffff' : '#939393'}/>
                    ),
                    tabBarLabel: ({focused}) => <Text style={{
                        color: focused ? '#ffffff' : '#939393'
                    }}>Search</Text>
                }}/>
                <Tab.Screen name="Profile" component={ProfileStack} options={{
                    tabBarIcon: ({focused}) => (
                        <Icon name={'settings'} style={{
                            fontSize: 20
                        }} color={focused ? '#ffffff' : '#939393'}/>
                    ),
                    tabBarLabel: ({focused}) => <Text style={{
                        color: focused ? '#ffffff' : '#939393'
                    }}>Profile</Text>
                }}/>
                <Tab.Screen name="Chat" component={ChatStack} options={{
                    tabBarIcon: ({focused}) => (
                        <Icon name={focused ? 'chatbubble' : 'chatbubble-outline'} style={{
                            fontSize: 20
                        }} color={focused ? '#ffffff' : '#939393'}/>
                    ),
                    tabBarLabel: ({focused}) => <Text style={{
                        color: focused ? '#ffffff' : '#939393'
                    }}>Chat</Text>,
                }}/>
            </Tab.Navigator>
        )
    }
}
