import LocalConstants from "../UIConstants.js";

async function fetchConstants(props) {
    const { constantsGroup } = props;
    
    // Open local UI Constants
    const combinedConstants = {...LocalConstants};
    
    // Load remote UI Constants
    const remoteConstants = await fetch(`/api/info/uiconstants`);
    
    // Take the most recent set of constants
    Object.entries(remoteConstants).forEach([key, value], idx => {
        if (key in combinedConstants) {
            combinedConstants[key] = combinedConstants[key].timestamp < value.timestamp ? value : combinedConstants[key];
        } else {
            combinedConstants[key] = value;
        }
    })
    
    // Return those constants
    return combinedConstants[constantsGroup].list;
}

export default fetchConstants;