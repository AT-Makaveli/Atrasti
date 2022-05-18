import React from 'react';
import {
    Animated,
    Pressable,
    StyleSheet,
    View
} from 'react-native';
import { useNavigation } from '@react-navigation/core';
import { useTheme } from '@react-navigation/native';
import BottomDrawerButton from "./BottomDrawerButton";
import Icon from "react-native-vector-icons/Ionicons";

export interface IBottomDrawer {
}

const BottomDrawer: React.FC<IBottomDrawer> = () => {
    const navigation = useNavigation();
    const {colors} = useTheme();

    // @ts-ignore
    // @ts-ignore
    const styles = StyleSheet.create({
        bottomView: {
            flex: 1,
            justifyContent: 'flex-end',
            alignItems: 'center',
        },
        modalView: {
            overflow: "hidden",
            paddingTop: 10,
            paddingBottom: 30,
            width: '100%',
            borderTopStartRadius: 20,
            borderTopEndRadius: 20,
            backgroundColor: colors.card,
        },
    });

    return (
        <View style={styles.bottomView}>
            <Pressable style={StyleSheet.absoluteFill} onPress={navigation.goBack}/>
            <Animated.View style={styles.modalView}>
                <BottomDrawerButton title={'Manage Profile'} onPress={() => {
                    navigation.goBack();
                    navigation.navigate('ManageProfileView');
                }} icon={
                    <Icon name={'settings-outline'} color={'#FFFFFF'} size={20} style={{
                        marginLeft: 10,
                        textAlignVertical: "center",
                        paddingRight: 5
                    }}/>
                }/>
                <BottomDrawerButton title={'Upload Product'} onPress={() => {
                    navigation.goBack();
                    navigation.navigate('UploadProductView');
                }} icon={
                    <Icon name={'watch-outline'} color={'#FFFFFF'} size={20} style={{
                        marginLeft: 10,
                        textAlignVertical: "center",
                        paddingRight: 5
                    }}/>
                }/>
            </Animated.View>
        </View>
    );
};

export default BottomDrawer;
