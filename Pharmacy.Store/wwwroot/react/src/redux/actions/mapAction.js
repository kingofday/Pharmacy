import actionTypes from './actionTypes';

export function SetLocationAction( lng,lat) {
    return {
        type: actionTypes.SET_LOCATION,
        payload: { lng, lat}
    };
};