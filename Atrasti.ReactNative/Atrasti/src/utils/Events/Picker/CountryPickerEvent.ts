export default class CountryPickerEvent {
    public static EVENT_TYPE: "COUNTRY_PICKER_SELECTED";

    public readonly country: string;
    public readonly state: string | null;

    constructor(country: string, state: string | null) {
        this.country = country;
        this.state = state;
    }
}
