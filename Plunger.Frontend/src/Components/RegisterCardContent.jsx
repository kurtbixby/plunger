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
import {objFromForm} from "../Utils.js";
import APICalls from "../APICalls.js";
import {useCurrentUser} from "../CurrentUserProvider.jsx";
import {isEmailValid, isPasswordValid, isUsernameValid} from "../InputValidation.js";

function RegisterCardContent(props) {
    const {handleBackClick} = props;

    const [cardFlowState, setCardFlowState] = useState(0);
    const [isLoading, setIsLoading] = useState(false);
    const [buttonSendState, setButtonSendState] = useState(0);
    
    const loadingMessages = ["Registering", "Signing In"];

    const { dispatch: userDispatch } = useCurrentUser();

    function handleBackArrow(e) {
        if (cardFlowState === 0) {
            handleBackClick();
        } else if (cardFlowState === 1) {
            setCardFlowState(0);
        } else {
            setCardFlowState(cardFlowState - 1);
        }
    }
    
    async function handleRegisterFlow(e) {
        e.preventDefault();
        
        const formData = objFromForm(e.target.form);

        const formValidity = validateFormInput(formData);

        if (!formValidity.isValid) {
            return;
        }

        setButtonSendState(0);
        const regStatus = await handleRegister(formData);
        if (regStatus === true) {
            setButtonSendState(1);
            await handleLogin(formData);
        }
    }

    async function handleRegister(formData) {
        // Add spinner
        setIsLoading(true);
        try {
            await APICalls.sendNewUserRequest({
                username: formData.username,
                email: formData.email,
                password: formData.password
            });
        } catch (error) {
            // Do stuff with error
            return false;
        } finally {
            setIsLoading(false);
        }
        
        return true;
    }

    async function handleLogin(formData) {
        setIsLoading(true);
        userDispatch({ type: "sendLoginRequest" });
        let userDetails = null;

        try {
            userDetails = await APICalls.sendLoginRequest({
                identity: formData.username,
                password: formData.password,
            });
        } catch (error) {
            // Do stuff with error
            userDispatch({
                type: "loginFailed",
            });
            return;
        } finally {
            setIsLoading(false);
        }

        if (userDetails != null) {
            userDispatch({
                type: "loginSucceeded",
                payload: { username: userDetails.userName, userId: userDetails.userId },
            });
        }
    }
    
    function validateFormInput(formData) {
        const { username, email, confirmEmail, password, confirmPassword} = formData;
        
        let validationResult = {
            isValid: true,
            fields: {}
        };
        
        // Check username presence, format validity, & availability
        if (!isUsernameValid(username)) {
            validationResult.isValid = false;
            validationResult.fields["username"] = "Invalid username";
        }
        
        // Check email presence & format validity
        if (!isEmailValid(email)) {
            validationResult.isValid = false;
            validationResult.fields["email"] = "Invalid email";
        }
        if (confirmEmail !== email) {
            // Emails must match
            validationResult.isValid = false;
            validationResult.fields["confirmEmail"] = "Email addresses must match";
        }
        
        // Check password presence & format validity
        if (!isPasswordValid(password)) {
            validationResult.isValid = false;
            validationResult.fields["password"] = "Invalid password";
        }
        if (confirmPassword !== password) {
            // Passwords must match
            validationResult.isValid = false;
            validationResult.fields["confirmPassword"] = "Passwords must match";
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
                    <Heading>Register</Heading>
                </Center>
            </Grid>
        </CardHeader>
        <CardBody>
            <Center>
                <form>
                    <Flex direction="column" gap="2">
                        <FormControl>
                            <FormLabel>Username</FormLabel>
                            <Input name="username" placeholder="ramza" size="lg"/>
                        </FormControl>
                        <FormControl>
                            <FormLabel>Email</FormLabel>
                            <Input name="email" type="email" placeholder="ramza@beouvle.info" size="lg"/>
                        </FormControl>
                        <FormControl>
                            <FormLabel>Confirm Email</FormLabel>
                            <Input name="confirmEmail" type="email" placeholder="ramza@beouvle.info" size="lg"/>
                        </FormControl>
                        <FormControl>
                            <FormLabel>Password</FormLabel>
                            <Input name="password" type="password" placeholder="hunter2" size="lg"/>
                        </FormControl>
                        <FormControl>
                            <FormLabel>Confirm Password</FormLabel>
                            <Input name="confirmPassword" type="password" placeholder="hunter2" size="lg"/>
                        </FormControl>
                        <Button isLoading={!!isLoading} loadingText={loadingMessages[buttonSendState]} onClick={handleRegisterFlow} w="100%">Register</Button>
                    </Flex>
                </form>
            </Center>
        </CardBody>
    </>
}

export default RegisterCardContent;