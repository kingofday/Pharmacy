import actionTypes from './actionTypes';

export function SetAddrssAction(address, reciever, recieverMobileNumber, deliveryId, deliveryCost) {
    return {
        type: actionTypes.SET_ADDRESS,
        payload: { address, reciever, recieverMobileNumber, deliveryId, deliveryCost}
    };
};