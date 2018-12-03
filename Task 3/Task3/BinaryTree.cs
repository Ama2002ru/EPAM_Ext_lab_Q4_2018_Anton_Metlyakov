namespace Task3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BinaryTreeNode
    {
        /// <summary>
        /// Constructor. Creates a node and places value inside
        /// </summary>
        /// <param name="val">just int </param>
        public BinaryTreeNode(int val)
        {
            this.Left = this.Right = null;
            this.Value = val;
            this.Count = 1;
        }

        private BinaryTreeNode Left { get; set; }

        private BinaryTreeNode Right { get; set; }

        private int Value { get; set; }

        private int Count { get; set; }
        
        /// <summary>
        /// Runs down exisitng tree and places value as new object
        /// </summary>
        /// <param name="val">just int</param>
        public void Add(int val)
        {
            this.Count++;
            if (val >= this.Value)
            {
  // Right branch 
                if (this.Right != null)
                {
 // recursive call
                    this.Right.Add(val);
                }
                else
                {
  // new Node
                    this.Right = new BinaryTreeNode(val);
                }
            }
            else
            {
 // Left branch
                if (this.Left != null)
                {
 // recursive call
                    this.Left.Add(val);
                }
                else
                {
  // new Node
                    this.Left = new BinaryTreeNode(val);
                }
            }
        }

        /// <summary>
        /// collect left and right part of a binary tree 
        /// </summary>
        /// <returns>array of ints</returns>
        public int[] GetSortedTree()
        {
            int[] sortedArray = new int[this.Count];
            int[] leftSubArray = this.Left is null ? new int[0] : this.Left.GetSortedTree();
            int[] rightSubArray = this.Right is null ? new int[0] : this.Right.GetSortedTree();
            leftSubArray.CopyTo(sortedArray, 0);
            sortedArray[leftSubArray.Length] = this.Value;
            rightSubArray.CopyTo(sortedArray, leftSubArray.Length + 1);
            return sortedArray;
        }
    }
}
