import {useCurrentUser} from "../CurrentUserProvider.jsx";
import UserHomePage from "./UserHomePage.jsx";
import {Button, Card, CardBody, CardHeader, Center, Container, Flex, Heading} from "@chakra-ui/react";
import {useState} from "react";
import WelcomeCardContent from "../Components/WelcomeCardContent.jsx";
import SignInCardContent from "../Components/SignInCardContent.jsx";
import RegisterCardContent from "../Components/RegisterCardContent.jsx";

function LandingPage() {
    const {
        state: { isLoading, isLoggedIn, user: currentUser },
    } = useCurrentUser();
    
    const [loginCardState, setLoginCardState] = useState(0);
    
    function handleSignInClick(e) {
        setLoginCardState(1);
    }
    
    function handleRegisterClick(e) {
        setLoginCardState(2);
    }
    
    function handleGoBack(e) {
        setLoginCardState(0);
    }
    
    return isLoggedIn === true ? <UserHomePage /> : <div>
            <p>Not Logged In</p>
            <Container minW="60%" centerContent>
                <Card w="md">
                    {loginCardState === 0 && <WelcomeCardContent handleSignInClick={handleSignInClick} handleRegisterClick={handleRegisterClick}/>}
                    {loginCardState === 1 && <SignInCardContent handleBackClick={handleGoBack}/>}
                    {loginCardState === 2 && <RegisterCardContent handleBackClick={handleGoBack}/>}
                    {/*<CardHeader>*/}
                    {/*    <Center>*/}
                    {/*        <Heading>Welcome</Heading>*/}
                    {/*    </Center>*/}
                    {/*</CardHeader>*/}
                    {/*<CardBody>*/}
                    {/*    <Center>*/}
                    {/*        <Flex minW="70%" justifyContent="space-around">*/}
                    {/*            <Button onClick={handleSignInClick}>Sign In</Button>*/}
                    {/*            <Button onClick={handleRegisterClick}>Register</Button>*/}
                    {/*        </Flex>*/}
                    {/*    </Center>*/}
                    {/*</CardBody>*/}
                </Card>
            </Container>
        </div>;
}

export default LandingPage;