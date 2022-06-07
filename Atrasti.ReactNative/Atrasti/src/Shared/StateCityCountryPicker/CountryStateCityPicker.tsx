import React from "react";
import { DeviceEventEmitter, View } from "react-native";
import CountryPicker from "./CountryPicker";
import StatePicker from "./StatePicker";
import CityPicker from "./CityPicker";
import CountryPickerEvent from "../../utils/Events/Picker/CountryPickerEvent";
import StatePickerEvent from "../../utils/Events/Picker/StatePickerEvent";

export interface CountryStateCityPickerProps {
    onValueChange: (country: string, state: string, city: string) => void;
    country: string | null,
    state: string | null,
    city: string
}

export interface CountryStateCityPickerState {
    country: string;
    state: string;
    city: string;
}

export default class CountryStateCityPicker extends React.Component<CountryStateCityPickerProps, CountryStateCityPickerState> {

    constructor(props: CountryStateCityPickerProps) {
        super(props);

        this.state = {
            country: props.country ?? '',
            state: props.state ?? '',
            city: props.city ?? ''
        }
    }

    componentDidUpdate(prevProps: Readonly<CountryStateCityPickerProps>, prevState: Readonly<CountryStateCityPickerState>, snapshot?: any) {
        this.props.onValueChange(this.state.country, this.state.state, this.state.city);
    }

    render() {
        return (
            <View>
                <CountryPicker country={this.state.country} onValueChange={country => {
                    let state = this.state.state;
                    if (country !== this.state.country) state = '';
                    this.setState({
                        country: country,
                        state: state
                    })
                }}/>
                <StatePicker country={this.state.country} state={this.state.state} onValueChange={state => {
                    let city = this.state.city;
                    if (state !== this.state.state) city = '';
                    this.setState({
                        state: state,
                        city: city
                    });
                }}/>
                <CityPicker country={this.state.country} state={this.state.state} city={this.state.city} onValueChange={city => {
                    this.setState({
                        city: city
                    })
                }} />
            </View>
        )
    }

}
