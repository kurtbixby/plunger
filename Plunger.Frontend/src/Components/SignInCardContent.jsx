import {
    Box,
    Button,
    CardBody,
    CardHeader,
    Center,
    Flex,
    FormControl,
    FormLabel,
    Grid,
    Heading,
    Input
} from "@chakra-ui/react";
import {ArrowBackIcon} from "@chakra-ui/icons";
import {useState} from "react";
import {isPasswordValid, isUsernameValid} from "../InputValidation.js";
import APICalls from "../APICalls.js";
import {objFromForm} from "../Utils.js";
import {useCurrentUser} from "../CurrentUserProvider.jsx";

function SignInCardContent(props) {
    const {handleBackClick} = props;
    
    const [cardState, setCardState] = useState(0);

    const { dispatch: userDispatch } = useCurrentUser();
    
    function handleBackArrow(e) {
        if (cardState === 0) {
            handleBackClick();
        } else if (cardState === 1) {
            setCardState(cardState - 1);
        }
    }
    
    async function handleSignIn(e) {
        e.preventDefault();
        
        const formData = objFromForm(e.target.form);

        const formValidity = validateFormInput(formData);

        if (!formValidity.isValid) {

            return;
        }
        
        userDispatch({ type: "sendLoginRequest" });
        const userDetails = await APICalls.sendLoginRequest({
            identity: formData.username,
            password: formData.password,
        });
        userDispatch({
            type: "loginSucceeded",
            payload: { username: userDetails.userName, userId: userDetails.userId },
        });
    }
    
    function validateFormInput(formData) {
        const { username, password } = formData;

        let validationResult = {
            isValid: true,
            fields: {}
        };

        // Check username presence, format validity, & availability
        if (!isUsernameValid(username)) {
            validationResult.isValid = false;
            validationResult.fields["username"] = "Invalid username";
        }

        // Check password presence & format validity
        if (!isPasswordValid(password)) {
            validationResult.isValid = false;
            validationResult.fields["password"] = "Invalid password";
        }
        
        return validationResult;
    }
    
    return <>
        <CardHeader>
            <Grid templateColumns="repeat(3, 1fr)">
                <Flex justify="start">
                    <Box as="button" onClick={handleBackArrow}>
                        <ArrowBackIcon boxSize={6} color="black"/>
                    </Box>
                </Flex>
                <Center>
                    <Heading>Sign In</Heading>
                </Center>
            </Grid>
        </CardHeader>
        <CardBody>
            <Center>
                <form>
                    <Flex direction="column" gap={2}>
                        <FormControl>
                            <FormLabel>Username</FormLabel>
                            <Input name="username" placeholder="ramza" size="lg"/>
                        </FormControl>
                        <FormControl>
                            <FormLabel>Password</FormLabel>
                            <Input name="password" type="password" placeholder="hunter2" size="lg"/>
                        </FormControl>
                        <Button onClick={handleSignIn} w="100%">Sign In</Button>
                    </Flex>
                </form>
            </Center>
        </CardBody>
    </>
}

export default SignInCardContent;