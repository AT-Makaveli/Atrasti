import React from "react";
import { Picker } from "@react-native-picker/picker";
import { DeviceEventEmitter, EmitterSubscription, StyleSheet, View } from "react-native";
import { CountriesAndCities } from "../utils/JsonUtils";
import CountryPickerEvent from "../utils/Events/Picker/CountryPickerEvent";
import { findCountry } from "../Api/CountryAPI";
import { InfoState_Res } from "../Api/Models/Responds/InfoState_Res";

export interface StatePickerProps {
    country: string,
    state: string,
    onValueChange: (country: string) => void
}

export interface StatePickerState {
    states: string[]
}

export default class StatePicker extends React.Component<StatePickerProps, StatePickerState> {

    private _onCountryChange: EmitterSubscription | null = null;

    constructor(props: StatePickerProps) {
        super(props);

        this.state = {
            states: []
        }
    }

    componentDidMount() {
        this._onCountryChange = DeviceEventEmitter.addListener(CountryPickerEvent.EVENT_TYPE, this.onCountryChange.bind(this));
        if(this.props.country === '') {
            this.setState({
                states: ['Please select country first']
            })
        }

        const country = findCountry(this.props.country);
        if(country !== null)
            this.setState({
                states: Object.keys(country.states)
            })
    }

    componentWillUnmount() {
        this._onCountryChange?.remove();
    }

    onCountryChange(event: CountryPickerEvent) {
        const country = findCountry(event.country);
        if(country !== null)
            this.setState({
                states: Object.keys(country.states)
            });
    }

    renderProductList() {
        return this.state.states.map((state, index) => {
            return <Picker.Item key={index} label={state} value={state}/>
        })
    }

    render() {
        return (
            <View style={Styles.parent}>
                <Picker style={Styles.selectPicker} selectedValue={this.props.state}
                        onValueChange={this.props.onValueChange}>
                    {this.renderProductList()}
                </Picker>
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    parent: {
        marginTop: 10,
        overflow: "hidden",
        borderRadius: 50
    },
    selectPicker: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        color: '#FFFFFF',
        padding: 10
    },
});
