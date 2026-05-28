//*****************************************************************************
//** 3093. Longest Common Suffix Queries                            leetcode **
//*****************************************************************************

/**
 * Note: The returned array must be malloced, assume caller calls free().
 */
typedef struct
{
    int child[26];

    int bestIndex;
    int bestLen;

} TrieNode;

int* stringIndices(char** wordsContainer,
                   int wordsContainerSize,
                   char** wordsQuery,
                   int wordsQuerySize,
                   int* returnSize)
{
    //
    // Max possible trie nodes:
    // total chars + root
    //
    int maxNodes = 1;

    for(int i = 0; i < wordsContainerSize; i++)
    {
        maxNodes += strlen(wordsContainer[i]);
    }

    TrieNode* trie = (TrieNode*)malloc(sizeof(TrieNode) * maxNodes);

    //
    // Initialize root
    //
    memset(&trie[0], 0, sizeof(TrieNode));

    trie[0].bestLen = 1 << 30;
    trie[0].bestIndex = -1;

    int nodes = 1;

    //
    // Build reversed trie
    //
    for(int i = 0; i < wordsContainerSize; i++)
    {
        char* foinf = wordsContainer[i];

        int len = strlen(foinf);

        //
        // Update root best
        //
        if(len < trie[0].bestLen ||
           (len == trie[0].bestLen && i < trie[0].bestIndex))
        {
            trie[0].bestLen = len;
            trie[0].bestIndex = i;
        }

        int cur = 0;

        for(int j = len - 1; j >= 0; j--)
        {
            int idx = foinf[j] - 'a';

            if(trie[cur].child[idx] == 0)
            {
                memset(&trie[nodes], 0, sizeof(TrieNode));

                trie[nodes].bestLen = 1 << 30;
                trie[nodes].bestIndex = -1;

                trie[cur].child[idx] = nodes++;
            }

            cur = trie[cur].child[idx];

            if(len < trie[cur].bestLen ||
               (len == trie[cur].bestLen && i < trie[cur].bestIndex))
            {
                trie[cur].bestLen = len;
                trie[cur].bestIndex = i;
            }
        }
    }

    int* retval = (int*)malloc(sizeof(int) * wordsQuerySize);

    //
    // Process queries
    //
    for(int i = 0; i < wordsQuerySize; i++)
    {
        char* foinf = wordsQuery[i];

        int len = strlen(foinf);

        int cur = 0;

        for(int j = len - 1; j >= 0; j--)
        {
            int idx = foinf[j] - 'a';

            if(trie[cur].child[idx] == 0)
            {
                break;
            }

            cur = trie[cur].child[idx];
        }

        retval[i] = trie[cur].bestIndex;
    }

    free(trie);

    *returnSize = wordsQuerySize;

    return retval;
}