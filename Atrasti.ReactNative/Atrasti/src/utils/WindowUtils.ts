import { Dimensions, ScaledSize } from "react-native";

const windowDimensions = Dimensions.get("window");

export function getWindowDimensions(): ScaledSize {
    return windowDimensions;
}
