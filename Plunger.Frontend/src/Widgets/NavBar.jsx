import {
    // Avatar,
    Box,
    Button,
    Flex,
    HStack, IconButton,
    Menu,
    MenuButton,
    MenuDivider,
    MenuItem,
    MenuList, Stack, Text,
    useColorModeValue, useDisclosure
} from "@chakra-ui/react";
import {AddIcon, CloseIcon, HamburgerIcon} from "@chakra-ui/icons";
import {useCurrentUser} from "../CurrentUserProvider.jsx";
import {Link} from "react-router-dom";

function NavItem(props) {
    const {children, link} = props;
    
    return <Box px={2} py={1} rounded="md" _hover={{
        textDecoration: "none",
        bg: useColorModeValue("gray.200", "gray.700"),
    }}>
        <Link to={link}>{children}</Link>
    </Box>
}

function NavBar(props) {
    const {title} = props;
    const {
        state: { isLoading, isLoggedIn, user },
    } = useCurrentUser();
    const { isOpen, onOpen, onClose } = useDisclosure();
    
    const links = getLinks(isLoggedIn);
    
    function getLinks(loggedIn) {
        let baseArray = [
            {
                id: 0,
                url: "/",
                text: title
            }
        ]; 
        if (loggedIn) {
            baseArray = baseArray.concat([
                {
                    id: 1,
                    url: "/" + user.username,
                    text: "My Profile"
                },
                {
                    id: 2,
                    url: "/" + user.username + "/collection",
                    text: "Collection"
                },
                {
                    id: 3,
                    url: "/" + user.username + "/gamestates",
                    text: "Game States"
                }
            ]);
        }
        
        return baseArray;
    }
    
    return <Box bg={useColorModeValue("gray.100", "gray.900")} px={4}>
        <Flex h={16} alignItems={"center"} justifyContent={"space-between"}>
            <IconButton size={"md"} aria-label={"Open Menu"} display={{md: "none"}} onClick={isOpen ? onClose : onOpen} icon={isOpen ? <CloseIcon /> : <HamburgerIcon />} />
            <HStack spacing={8} alignItems={"center"}>
                <HStack as={"nav"} spacing={4} display={{base: "none", md: "flex"}}>
                    {links.map(l => <NavItem key={l.id} link={l.url}>{l.text}</NavItem>)}
                </HStack>
            </HStack>
        <Flex alignItems={"center"}>
            {!isLoggedIn ? <Button variant={"solid"} colorScheme={"teal"} size={"sm"} mr={4}>Login</Button> : (<>
            <Button variant={"solid"} colorScheme={"teal"} size={"sm"} mr={4} leftIcon={<AddIcon/>}>Add Game</Button>
            <Menu>
                <MenuButton as={Button} rounded={"full"} variant={"link"} cursor={"pointer"} minW={0}>
                    {/*<Avatar*/}
                    {/*    size={"sm"}*/}
                    {/*    src={avatarIcon}*/}
                    {/*/>*/}
                    <Text fontSize={'lg'}>{user.username}</Text>
                </MenuButton>
                <MenuList>
                    {/*<MenuDivider/>*/}
                    <MenuItem>
                        <Link to={"/logout"}>Log Out</Link>
                    </MenuItem>
                </MenuList>
            </Menu></>)}
        </Flex>
        </Flex>

        {isOpen ? (
            <Box>
                <Stack as={"nav"} spacing={4}>
                    {links.map(l => <NavItem key={l.id} link={l.url}>{l.text}</NavItem>)}
                </Stack>
            </Box>
        ) : null}
    </Box>
}

export default NavBar;