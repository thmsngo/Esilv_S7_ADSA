#Library
from random import seed
from random import randint,sample

#Creation of the AVL Tree and Player class to manage database
# Generic player class that will gather useful informations of player
class Player:
    #Construtor
    def __init__(self,player):
        self.player = player
        self.id = player["id"]
        self.score = player["score"]
        self.scores = player["scores"]#the list of scores of every games
        #self.nb_play = 0 #in order to calculate score
        #self.sumscores = 0 #the addition of every scores 
        self.left = None
        self.right = None
        self.height = 1
    def display(self):
        lines, *_ = self._display_aux()
        for line in lines:
            print(line)
#player["name"] + " " + str(player["scores"][0])
    def _display_aux(self):
        data = self.id + " " + str(self.score)
        if self.right is None and self.left is None:
            line = '%s' % data
            width = len(line)
            height = 1
            middle = width // 2
            return [line], width, height, middle
        if self.right is None:
            lines, n, p, x = self.left._display_aux()
            s = '%s' % data
            u = len(s)
            first_line = (x + 1) * ' ' + (n - x - 1) * '_' + s
            second_line = x * ' ' + '/' + (n - x - 1 + u) * ' '
            shifted_lines = [line + u * ' ' for line in lines]
            return [first_line, second_line] + \
                    shifted_lines, n + u, p + 2, n + u // 2
        if self.left is None:
            lines, n, p, x = self.right._display_aux()
            s = '%s' % data
            u = len(s)
            first_line = s + x * '_' + (n - x) * ' '
            second_line = (u + x) * ' ' + '\\' + (n - x - 1) * ' '
            shifted_lines = [u * ' ' + line for line in lines]
            return [first_line, second_line] + \
                    shifted_lines, n + u, p + 2, u // 2
        left, n, p, x = self.left._display_aux()
        right, m, q, y = self.right._display_aux()
        s = '%s' % data
        u = len(s)
        first_line = (x + 1) * ' ' \
                      + (n - x - 1) * '_' \
                      + s + y * '_' \
                      + (m - y) * ' ' 
        second_line = x * ' ' + '/' \
                      + (n - x - 1 + u + y) * ' ' \
                      + '\\' + (m - y - 1) * ' '
        if p < q:
            left += [n * ' '] * (q - p)
        elif q < p:
            right += [m * ' '] * (p - q)
        zipped_lines = zip(left, right)
        lines = [first_line, second_line] \
                + [a + u * ' ' + b for a, b in zipped_lines]
        return lines, n + m + u, max(p, q) + 2, n + u // 2

# AVL tree class including insert() and delete() function
class AVL_Tree(object):

    ## Recursive function to insert a player node in our tree 
    def insert(self, root, player): #have to specify the root and the player
      
        # Step 1 - Perform normal BST 
        if root is None: 
            return player 
        elif player.score < root.score: 
            root.left = self.insert(root.left, player)
        else: 
            root.right = self.insert(root.right, player) 
  
        # Step 2 - Update the height of the  
        # ancestor node 
        root.height = 1 + max(self.getHeight(root.left), 
                           self.getHeight(root.right)) 
  
        # Step 3 - Get the balance factor 
        balance = self.getBalance(root) 
  
        # Step 4 - If the node is unbalanced,  
        # then try out the 4 cases 
        # Case 1 - Left Left 
        if balance > 1 and player.score <= root.left.score: 
            return self.rightRotate(root) 
  
        # Case 2 - Right Right 
        if balance < -1 and player.score >= root.right.score: 
            return self.leftRotate(root) 
  
        # Case 3 - Left Right 
        if balance > 1 and player.score > root.left.score: 
            root.left = self.leftRotate(root.left) 
            return self.rightRotate(root) 
  
        # Case 4 - Right Left 
        if balance < -1 and player.score < root.right.score: 
            root.right = self.rightRotate(root.right) 
            return self.leftRotate(root) 
  
        return root 

    # Recursive function to delete a player node 
    def delete(self, root, player): 
  
        # Step 1 - Perform standard BST delete 
        if not root: 
            return root 
        elif player.score <= root.score and root.left is not None: 
            root.left = self.delete(root.left, player) 
        elif player.score > root.score: 
            root.right = self.delete(root.right, player) 
        else: 
            if root.left is None: 
                temp = root.right 
                root = None
                return temp 
            elif root.right is None: 
                temp = root.left 
                root = None
                return temp 
            temp = self.getMinValueNode(root.right)
            root.score = temp.score 
            root.right = self.delete(root.right, temp) 
  
        # If the tree has only one node, 
        # simply return it 
        if root is None: 
            return root 
  
        # Step 2 - Update the height of the  
        # ancestor node 
        root.height = 1 + max(self.getHeight(root.left), 
                            self.getHeight(root.right)) 
  
        # Step 3 - Get the balance factor 
        balance = self.getBalance(root) 
  
        # Step 4 - If the node is unbalanced,  
        # then try out the 4 cases 
        # Case 1 - Left Left 
        if balance > 1 and self.getBalance(root.left) >= 0: 
            return self.rightRotate(root) 
  
        # Case 2 - Right Right 
        if balance < -1 and self.getBalance(root.right) <= 0: 
            return self.leftRotate(root) 
  
        # Case 3 - Left Right 
        if balance > 1 and self.getBalance(root.left) < 0: 
            root.left = self.leftRotate(root.left) 
            return self.rightRotate(root) 
  
        # Case 4 - Right Left 
        if balance < -1 and self.getBalance(root.right) > 0: 
            root.right = self.rightRotate(root.right) 
            return self.leftRotate(root) 
  
        return root 

    def leftRotate(self, z): 
  
        y = z.right 
        T2 = y.left 
  
        # Perform rotation 
        y.left = z 
        z.right = T2 
  
        # Update heights 
        z.height = 1 + max(self.getHeight(z.left), 
                         self.getHeight(z.right)) 
        y.height = 1 + max(self.getHeight(y.left), 
                         self.getHeight(y.right)) 
  
        # Return the new root 
        return y 
  
    def rightRotate(self, z): 
  
        y = z.left 
        T3 = y.right 
  
        # Perform rotation 
        y.right = z 
        z.left = T3 
  
        # Update heights 
        z.height = 1 + max(self.getHeight(z.left), 
                        self.getHeight(z.right)) 
        y.height = 1 + max(self.getHeight(y.left), 
                        self.getHeight(y.right)) 
  
        # Return the new root 
        return y 
  
    def getHeight(self, root): 
        if not root: 
            return 0
  
        return root.height 
  
    def getBalance(self, root): 
        if not root: 
            return 0
  
        return self.getHeight(root.left) - self.getHeight(root.right) 
  
    def getMinValueNode(self, root): 
        if root is None or root.left is None: 
            return root 
  
        return self.getMinValueNode(root.left)

    def preOrder(self, root): 
  
        if not root: 
            return
  
        print("{0} ".format(root.val), end="") 
        self.preOrder(root.left) 
        self.preOrder(root.right) 


"""
1| Propose a data structure to represent a Player and its Score
"""
#Initialize // Creation of variables

#Creation of a list of dictionaries that contains players' id and score
#Have all informations and possibility to remove eliminated players from list
players=[]
for i in range(100):
    player={"id": "player_{}".format(i),"score": 0,"scores" : []}
    players.append(player)

"""
2| Propose a most optimized data structures for the tournament
"""
#Creation of the avl-tree that contain every player nodes
database = AVL_Tree()
root_db = None
for player in players:
    root_db = database.insert(root_db, Player(player))

"""
3| Present and argue about a method that randomize player score at each game
"""
def RandomizeScore(players):
    for player in players:
        player["scores"].append(randint(0,12)) #Add value to list of scores
        player["score"] = round(sum(player["scores"])/len(player["scores"]),2) #calculate the score of the player based of his scores
    return players

"""
4| Present and argue about a method to update Players score and the database
"""
players = []
#Inorder traversing for our avl tree
#Recursive function to first get worst players, then the best
def inorderTraversal(root):
    if root:
        inorderTraversal(root.left)
        player = {"id": root.id, "score": root.score, "scores": root.scores}
        players.append(player)
        #print(root.id,root.score,root.scores)
        inorderTraversal(root.right)

inorderTraversal(root_db)

#players = RandomizeScore(players)
database = AVL_Tree()
root_db = None
for player in players:
    root_db = database.insert(root_db, Player(player))
#root_db.display()

"""
5| Present and argue about a method to create random games based on the database
"""
#Because games based of the database are the ones where there aren't any removed player
# and that they are numeroted from 0 to 99
# we will just randomly work on a list of integer from 0 to 100
def RandomGames(players):
    #Creation of 10 nodes and AVL trees
    database_roots = []
    databases = []
    for _ in range(10):
        database_roots.append(None)
        databases.append(AVL_Tree())

    #Sampling without replacement using sample() of random library
    order = sample(range(100), 100) 

    #Fill our 10 AVL tree with random players
    for index in range(len(order)):
        database_roots[index//10] = databases[index//10].insert(database_roots[index//10], Player(players[order[index]]))

"""
6| Present and argue about a method to create games based on the ranking
"""
def RankingGames(players):

    #Creation of X nodes and AVL trees
    # X represent the name of games based on ranking
    database_roots = []
    databases = []
    for _ in range(int(len(players) / 10)):
        database_roots.append(None)
        databases.append(AVL_Tree())

    #Fill the AVL Tree based on ranking
    for index in range(len(players)):
        database_roots[index//10] = databases[index//10].insert(database_roots[index//10], Player(players[index]))

"""
7| Present and argue about a method to drop the players and to play game until the last 10 players
"""
#Add 3 scores and create random games based on database
for _ in range(3):
    players = RandomizeScore(players)
#Update database
database = AVL_Tree()
root_db = None
for player in players:
    root_db = database.insert(root_db, Player(player))
players=[]
inorderTraversal(root_db)
RandomGames(players)

#9 x Games based on ranking with -10 players at the end of each game
for _ in range(9):
    players = RandomizeScore(players)

#Update database
database = AVL_Tree()
root_db = None
for player in players:
    root_db = database.insert(root_db, Player(player))
players=[]
inorderTraversal(root_db)

for _ in range(10):
    players = []
    inorderTraversal(root_db)
    for i in range(10):
        root_db = database.delete(root_db, database.getMinValueNode(root_db))

#Update database
database = AVL_Tree()
root_db = None
for player in players:
    root_db = database.insert(root_db, Player(player))
players=[]
inorderTraversal(root_db)

"""
8| Present and argue about a method which display the TOP10 players and the podium after the final game.
"""
def display():
    #Display leaderboard
    print('\nStep 1: To organize the tournament')
    print('\n\nOur database at the final match:')
    root_db.display()
    print('\n\n\tTop 10 players:')
    for index in range(10):
        print((index+1),'|', players[9-index]["id"], ' with a score of ', players[9-index]["score"])
    print('\n\n\tPodium:')
    for index in range(3):
        print((index+1),'|', players[9-index]["id"], ' with a score of ', players[9-index]["score"])

if __name__ == "__main__":
    display()
