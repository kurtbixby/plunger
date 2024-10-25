import {Button, CardBody, CardHeader, Center, Flex, Heading} from "@chakra-ui/react";

function WelcomeCardContent(props) {
    const {handleSignInClick, handleRegisterClick} = props;
    
    return <>
        <CardHeader>
            <Center>
                <Heading>Welcome</Heading>
            </Center>
        </CardHeader>
        <CardBody>
            <Center>
                <Flex minW="70%" justifyContent="space-around">
                    <Button onClick={handleSignInClick}>Sign In</Button>
                    <Button onClick={handleRegisterClick}>Register</Button>
                </Flex>
            </Center>
        </CardBody>
    </>
}

export default WelcomeCardContent;